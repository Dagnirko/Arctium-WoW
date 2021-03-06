﻿/*
 * Copyright (C) 2012-2014 Arctium Emulation <http://arctium.org>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Framework.Network.Packets;
using RealmServer.Attributes;
using RealmServer.Constants.Net;

namespace RealmServer.Network.Packets.Handlers
{
    class AuthHandler
    {
        //TODO Implement field handling.
        public static void HandleAuthChallenge(RealmSession session)
        {
            var authChallenge = new Packet(ServerMessages.AuthChallenge);

            authChallenge.Write<ushort>(0);
            authChallenge.Write<byte>(1);     // DosZeroBits

            for (int i = 0; i < 8; i++)
                authChallenge.Write<uint>(0); // DosChallenge

            authChallenge.Write<uint>(0);     // Challenge

            session.Send(authChallenge);
        }

        [Message(ClientMessages.AuthSession)]
        public static void OnAuthSession(Packet packet, RealmSession session)
        {
            var digest = new byte[20];

            var realmId = packet.Read<uint>();

            digest[14] = packet.Read<byte>();

            var localChallenge = packet.Read<uint>();

            digest[0]  = packet.Read<byte>();
            digest[6]  = packet.Read<byte>();
            digest[2]  = packet.Read<byte>();
            digest[15] = packet.Read<byte>();
            digest[9]  = packet.Read<byte>();
            digest[8]  = packet.Read<byte>();
            digest[19] = packet.Read<byte>();
            digest[17] = packet.Read<byte>();

            var loginServerType = packet.Read<sbyte>();

            digest[1]  = packet.Read<byte>();
            digest[3]  = packet.Read<byte>();
            digest[12] = packet.Read<byte>();
            digest[10] = packet.Read<byte>();
            digest[4]  = packet.Read<byte>();
            digest[7]  = packet.Read<byte>();

            var build       = packet.Read<short>();
            var dosResponse = packet.Read<ulong>();

            digest[11] = packet.Read<byte>();
            digest[13] = packet.Read<byte>();

            var buildType = packet.Read<sbyte>();

            digest[18] = packet.Read<byte>();

            var loginServerId = packet.Read<uint>();
            var siteId        = packet.Read<uint>();
            var regionId      = packet.Read<uint>();

            digest[16] = packet.Read<byte>();
            digest[5]  = packet.Read<byte>();

            // AddonInfo stuff
            var packedAddonSize   = packet.Read<int>();
            var unpackedAddonSize = packet.Read<int>();
            var unknown           = packet.Read<ushort>();
            var packedAddonData   = packet.ReadBytes(packedAddonSize - 4);

            var account = packet.ReadString(11);

            //TODO Implement security checks & field handling.
            //TODO Implement AuthResponse.

            AddonHandler.LoadAddonInfoData(session, packedAddonData, packedAddonSize, unpackedAddonSize);
        }

        public static void HandleAuthResponse(RealmSession session)
        {

        }
    }
}