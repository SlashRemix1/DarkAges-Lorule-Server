﻿///************************************************************************
//Project Lorule: A Dark Ages Server (http://darkages.creatorlink.net/index/)
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
namespace Darkages.Network.ServerFormats
{
    public class ServerFormat13 : NetworkFormat
    {
        public ServerFormat13()
        {
        }

        public ServerFormat13(int serial, byte health)
        {
            Serial = serial;
            Health = health;
            Sound = 0xFF;
        }

        public ServerFormat13(int serial, byte health, byte sound)
        {
            Serial = serial;
            Health = health;
            Sound = sound;
        }

        public ServerFormat13(int source, int serial, byte health, byte sound)
        {
            Serial = serial;
            Health = health;
            Sound = sound;
        }

        public override bool Secured => true;

        public override byte Command => 0x13;

        public override bool Throttle => true;

        public int Source { get; set; }
        public int Serial { get; set; }
        public ushort Health { get; set; }
        public byte Sound { get; set; }

        public override void Serialize(NetworkPacketReader reader)
        {
        }

        public override void Serialize(NetworkPacketWriter writer)
        {
            writer.Write(Serial);
            writer.Write(Health);
            writer.Write(Sound);
        }
    }
}