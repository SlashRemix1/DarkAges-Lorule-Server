﻿#region

using System;
using System.Collections.ObjectModel;
using System.IO;
using Darkages.Compression;
using Darkages.IO;
using ServiceStack;
using ServiceStack.Text;

#endregion

namespace Darkages.Types
{
    public class MetafileManager
    {
        private static readonly MetafileCollection Metafiles = new MetafileCollection(4096);

        static MetafileManager()
        {
            var files = Directory.GetFiles(Path.Combine(ServerContext.StoragePath, "metafile"));
            Metafiles = new MetafileCollection(short.MaxValue);

            foreach (var file in files)
            {
                var metaFile = CompressableObject.Load<Metafile>(file);

                if (metaFile.Name.StartsWith("SClass"))
                {
                    continue;
                }

                if (metaFile.Name.StartsWith("ItemInfo"))
                {
                    continue;
                }

                Metafiles.Add(metaFile);
            }

            CreateFromTemplates();
        }

        public static Metafile GetMetaFile(string name)
        {
            return Metafiles.Find(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public static MetafileCollection GetMetaFiles()
        {
            return Metafiles;
        }

        private static void CompileTemplate(Metafile metaFile)
        {
            using (var stream = new MemoryStream())
            {
                metaFile.Save(stream);
                metaFile.InflatedData = stream.ToArray();
            }

            metaFile.Hash = Crc32Provider.ComputeChecksum(metaFile.InflatedData);
            metaFile.Compress();
        }

        private static void CreateFromTemplates()
        {
            GenerateItemInfoMeta();
            GenerateClassMeta();
        }

        private static void GenerateClassMeta()
        {
            var sclass1 = new Metafile { Name = "SClass1", Nodes = new Collection<MetafileNode>() };
            var sclass2 = new Metafile { Name = "SClass2", Nodes = new Collection<MetafileNode>() };
            var sclass3 = new Metafile { Name = "SClass3", Nodes = new Collection<MetafileNode>() };
            var sclass4 = new Metafile { Name = "SClass4", Nodes = new Collection<MetafileNode>() };
            var sclass5 = new Metafile { Name = "SClass5", Nodes = new Collection<MetafileNode>() };

            sclass1.Nodes.Add(new MetafileNode("Skill", ""));
            sclass2.Nodes.Add(new MetafileNode("Skill", ""));
            sclass3.Nodes.Add(new MetafileNode("Skill", ""));
            sclass4.Nodes.Add(new MetafileNode("Skill", ""));
            sclass5.Nodes.Add(new MetafileNode("Skill", ""));

            foreach (var (k, template) in ServerContext.GlobalSkillTemplateCache)
            {
                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Warrior)
                    sclass1.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Rogue)
                    sclass2.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Wizard)
                    sclass3.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Priest)
                    sclass4.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Monk)
                    sclass5.Nodes.Add(new MetafileNode(k, template.GetMetaData()));
            }

            sclass1.Nodes.Add(new MetafileNode("Skill_End", ""));
            sclass1.Nodes.Add(new MetafileNode("", ""));
            sclass1.Nodes.Add(new MetafileNode("Spell", ""));

            sclass2.Nodes.Add(new MetafileNode("Skill_End", ""));
            sclass2.Nodes.Add(new MetafileNode("", ""));
            sclass2.Nodes.Add(new MetafileNode("Spell", ""));

            sclass3.Nodes.Add(new MetafileNode("Skill_End", ""));
            sclass3.Nodes.Add(new MetafileNode("", ""));
            sclass3.Nodes.Add(new MetafileNode("Spell", ""));

            sclass4.Nodes.Add(new MetafileNode("Skill_End", ""));
            sclass4.Nodes.Add(new MetafileNode("", ""));
            sclass4.Nodes.Add(new MetafileNode("Spell", ""));

            sclass5.Nodes.Add(new MetafileNode("Skill_End", ""));
            sclass5.Nodes.Add(new MetafileNode("", ""));
            sclass5.Nodes.Add(new MetafileNode("Spell", ""));

            foreach (var (k, template) in ServerContext.GlobalSpellTemplateCache)
            {
                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Warrior)
                    sclass1.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Rogue)
                    sclass2.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Wizard)
                    sclass3.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Priest)
                    sclass4.Nodes.Add(new MetafileNode(k, template.GetMetaData()));

                if (template.Prerequisites != null && template.Prerequisites.Class_Required == Class.Monk)
                    sclass5.Nodes.Add(new MetafileNode(k, template.GetMetaData()));
            }

            sclass1.Nodes.Add(new MetafileNode("Spell_End", ""));
            sclass2.Nodes.Add(new MetafileNode("Spell_End", ""));
            sclass3.Nodes.Add(new MetafileNode("Spell_End", ""));
            sclass4.Nodes.Add(new MetafileNode("Spell_End", ""));
            sclass5.Nodes.Add(new MetafileNode("Spell_End", ""));

            CompileTemplate(sclass1);
            CompileTemplate(sclass2);
            CompileTemplate(sclass3);
            CompileTemplate(sclass4);
            CompileTemplate(sclass5);

            Metafiles.Add(sclass1);
            Metafiles.Add(sclass2);
            Metafiles.Add(sclass3);
            Metafiles.Add(sclass4);
            Metafiles.Add(sclass5);
        }

        private static void GenerateItemInfoMeta()
        {
            var i = 0;
            foreach (var batch in ServerContext.GlobalItemTemplateCache.BatchesOf(712))
            {
                var metaFile = new Metafile { Name = $"ItemInfo{i}", Nodes = new Collection<MetafileNode>() };

                foreach (var (k, template) in batch)
                {
                    if (template.Gender == 0)
                        continue;

                    var meta = template.GetMetaData();
                    metaFile.Nodes.Add(new MetafileNode(k, meta));
                }

                CompileTemplate(metaFile);
                Metafiles.Add(metaFile);
                i++;
            }
        }
    }
}