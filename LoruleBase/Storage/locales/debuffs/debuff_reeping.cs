﻿///************************************************************************
//Project Lorule: A Dark Ages Client (http://darkages.creatorlink.net/index/)
//Copyright(C) 2018 TrippyInc Pty Ltd
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//*************************************************************************/

using System;
using Darkages.Network.ServerFormats;
using Darkages.Types;

namespace Darkages.Storage.locales.debuffs
{
    public class debuff_reeping : Debuff
    {
        public readonly Random _rnd = new Random();
        public override string Name => "skulled";
        public override byte Icon => 89;
        public override int Length => ServerContextBase.Config.SkullLength;

        public string[] Messages =>
            ServerContextBase.Config.ReapMessage.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

        public int Count => Messages.Length;

        public override void OnApplied(Sprite Affected, Debuff debuff)
        {
            /* GM Character's don't die. */
            if (Affected is Aisling)
                if ((Affected as Aisling).GameMaster)
                    return;


            base.OnApplied(Affected, debuff);

            if (Affected.CurrentMapId == ServerContextBase.Config.DeathMap)
            {
                base.OnEnded(Affected, debuff);
                return;
            }

            if (Affected is Aisling)
            {
                (Affected as Aisling)
                    .Client
                    .SendAnimation(24,
                        (Affected as Aisling).Client.Aisling,
                        (Affected as Aisling).Client.Aisling.Target ??
                        (Affected as Aisling).Client.Aisling);

                var hpbar = new ServerFormat13
                {
                    Serial = Affected.Serial,
                    Health = 255,
                    Sound = 6
                };

                (Affected as Aisling).Show(Scope.Self, hpbar);
            }
            else
            {
                var nearby = Affected.GetObjects<Aisling>(Affected.Map, i => i.WithinRangeOf(Affected));

                foreach (var near in nearby)
                    near.Client.SendAnimation(24, Affected, Affected);
            }
        }

        public override void OnDurationUpdate(Sprite Affected, Debuff debuff)
        {
            if (Affected.CurrentMapId == ServerContextBase.Config.DeathMap)
            {
                base.OnEnded(Affected, debuff);
                return;
            }

            if (Affected is Aisling)
            {
                (Affected as Aisling)
                    .Client.SendAnimation(24,
                        (Affected as Aisling).Client.Aisling,
                        (Affected as Aisling).Client.Aisling.Target ??
                        (Affected as Aisling).Client.Aisling);

                var hpbar = new ServerFormat13
                {
                    Serial = Affected.Serial,
                    Health = 255,
                    Sound = 6
                };

                (Affected as Aisling).Show(Scope.Self, hpbar);


                (Affected as Aisling)
                    .Client
                    .SendMessage(0x02, Messages[_rnd.Next(Count) % Messages.Length]);
            }
            else
            {
                var nearby = Affected.GetObjects<Aisling>(Affected.Map, i => Affected.WithinRangeOf(i));

                foreach (var near in nearby)
                {
                    if (near == null || near.Client == null)
                        continue;

                    if (Affected == null)
                        continue;

                    var client = near.Client;
                    client.SendAnimation(24, Affected, client.Aisling);
                }
            }

            base.OnDurationUpdate(Affected, debuff);
        }

        public override void OnEnded(Sprite Affected, Debuff debuff)
        {
            if (Affected.CurrentMapId == ServerContextBase.Config.DeathMap)
            {
                base.OnEnded(Affected, debuff);
                return;
            }

            if (Affected is Aisling && !debuff.Cancelled)
            {
                (Affected as Aisling)
                    .Client
                    .SendMessage(0x02, "You have died.");

                var hpbar = new ServerFormat13
                {
                    Serial = Affected.Serial,
                    Health = 255,
                    Sound = 5
                };

                (Affected as Aisling).Show(Scope.Self, hpbar);
                (Affected as Aisling).Flags = AislingFlags.Dead;
                (Affected as Aisling).CastDeath();
                (Affected as Aisling).SendToHell();
            }

            base.OnEnded(Affected, debuff);
        }
    }
}