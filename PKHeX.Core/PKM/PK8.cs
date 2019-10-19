﻿using System;
using System.Collections.Generic;

namespace PKHeX.Core
{
    /// <summary> Generation 8 <see cref="PKM"/> format. </summary>
    public sealed class PK8 : _K6, IRibbonSetEvent3, IRibbonSetEvent4, IRibbonSetCommon3, IRibbonSetCommon4, IRibbonSetCommon6, IRibbonSetCommon7, IContestStats, IHyperTrain, IGeoTrack
    {
        private static readonly byte[] Unused =
        {
        };

        public override IReadOnlyList<byte> ExtraBytes => Unused;
        public override int Format => 8;
        public override PersonalInfo PersonalInfo => PersonalTable.USUM.GetFormeEntry(Species, AltForm);

        public override byte[] Data { get; }
        public PK8() => Data = new byte[PKX.SIZE_8PARTY];

        public PK8(byte[] data)
        {
            PKX.CheckEncrypted(ref data, Format);
            if (data.Length != PKX.SIZE_8PARTY)
                Array.Resize(ref data, PKX.SIZE_8PARTY);
            Data = data;
        }

        public override PKM Clone() => new PK8((byte[])Data.Clone()) { Identifier = Identifier };

        private string GetString(int Offset, int Count) => StringConverter.GetString7(Data, Offset, Count);
        private byte[] SetString(string value, int maxLength, bool chinese = false) => StringConverter.SetString7(value, maxLength, Language, chinese: chinese);

        // Structure
        #region Block A
        public override uint EncryptionConstant
        {
            get => BitConverter.ToUInt32(Data, 0x00);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x00);
        }

        public override ushort Sanity
        {
            get => BitConverter.ToUInt16(Data, 0x04);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x04);
        }

        public override ushort Checksum
        {
            get => BitConverter.ToUInt16(Data, 0x06);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x06);
        }

        public override int Species
        {
            get => BitConverter.ToUInt16(Data, 0x08);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x08);
        }

        public override int HeldItem
        {
            get => BitConverter.ToUInt16(Data, 0x0A);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0A);
        }

        public override int TID
        {
            get => BitConverter.ToUInt16(Data, 0x0C);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0C);
        }

        public override int SID
        {
            get => BitConverter.ToUInt16(Data, 0x0E);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x0E);
        }

        public override uint EXP
        {
            get => BitConverter.ToUInt32(Data, 0x10);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x10);
        }

        public override int Ability { get => Data[0x14]; set => Data[0x14] = (byte)value; }
        public override int AbilityNumber { get => Data[0x15] & 7; set => Data[0x15] = (byte)((Data[0x15] & ~7) | (value & 7)); }
        public override int MarkValue { get => BitConverter.ToUInt16(Data, 0x16); protected set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x16); }

        public override uint PID
        {
            get => BitConverter.ToUInt32(Data, 0x18);
            set => BitConverter.GetBytes(value).CopyTo(Data, 0x18);
        }

        public override int Nature { get => Data[0x1C]; set => Data[0x1C] = (byte)value; }
        public override bool FatefulEncounter { get => (Data[0x1D] & 1) == 1; set => Data[0x1D] = (byte)((Data[0x1D] & ~0x01) | (value ? 1 : 0)); }
        public override int Gender { get => (Data[0x1D] >> 1) & 0x3; set => Data[0x1D] = (byte)((Data[0x1D] & ~0x06) | (value << 1)); }
        public override int AltForm { get => Data[0x1D] >> 3; set => Data[0x1D] = (byte)((Data[0x1D] & 0x07) | (value << 3)); }
        public override int EV_HP { get => Data[0x1E]; set => Data[0x1E] = (byte)value; }
        public override int EV_ATK { get => Data[0x1F]; set => Data[0x1F] = (byte)value; }
        public override int EV_DEF { get => Data[0x20]; set => Data[0x20] = (byte)value; }
        public override int EV_SPE { get => Data[0x21]; set => Data[0x21] = (byte)value; }
        public override int EV_SPA { get => Data[0x22]; set => Data[0x22] = (byte)value; }
        public override int EV_SPD { get => Data[0x23]; set => Data[0x23] = (byte)value; }
        public int CNT_Cool { get => Data[0x24]; set => Data[0x24] = (byte)value; }
        public int CNT_Beauty { get => Data[0x25]; set => Data[0x25] = (byte)value; }
        public int CNT_Cute { get => Data[0x26]; set => Data[0x26] = (byte)value; }
        public int CNT_Smart { get => Data[0x27]; set => Data[0x27] = (byte)value; }
        public int CNT_Tough { get => Data[0x28]; set => Data[0x28] = (byte)value; }
        public int CNT_Sheen { get => Data[0x29]; set => Data[0x29] = (byte)value; }
        public byte ResortEventStatus { get => Data[0x2A]; set => Data[0x2A] = value; }
        private byte PKRS { get => Data[0x2B]; set => Data[0x2B] = value; }
        public override int PKRS_Days { get => PKRS & 0xF; set => PKRS = (byte)((PKRS & ~0xF) | value); }
        public override int PKRS_Strain { get => PKRS >> 4; set => PKRS = (byte)((PKRS & 0xF) | value << 4); }
        private byte ST1 { get => Data[0x2C]; set => Data[0x2C] = value; }
        public bool Unused0 { get => (ST1 & (1 << 0)) == 1 << 0; set => ST1 = (byte)((ST1 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool Unused1 { get => (ST1 & (1 << 1)) == 1 << 1; set => ST1 = (byte)((ST1 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool SuperTrain1_SPA { get => (ST1 & (1 << 2)) == 1 << 2; set => ST1 = (byte)((ST1 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool SuperTrain1_HP { get => (ST1 & (1 << 3)) == 1 << 3; set => ST1 = (byte)((ST1 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool SuperTrain1_ATK { get => (ST1 & (1 << 4)) == 1 << 4; set => ST1 = (byte)((ST1 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool SuperTrain1_SPD { get => (ST1 & (1 << 5)) == 1 << 5; set => ST1 = (byte)((ST1 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool SuperTrain1_SPE { get => (ST1 & (1 << 6)) == 1 << 6; set => ST1 = (byte)((ST1 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool SuperTrain1_DEF { get => (ST1 & (1 << 7)) == 1 << 7; set => ST1 = (byte)((ST1 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        private byte ST2 { get => Data[0x2D]; set => Data[0x2D] = value; }
        public bool SuperTrain2_SPA { get => (ST2 & (1 << 0)) == 1 << 0; set => ST2 = (byte)((ST2 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool SuperTrain2_HP { get => (ST2 & (1 << 1)) == 1 << 1; set => ST2 = (byte)((ST2 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool SuperTrain2_ATK { get => (ST2 & (1 << 2)) == 1 << 2; set => ST2 = (byte)((ST2 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool SuperTrain2_SPD { get => (ST2 & (1 << 3)) == 1 << 3; set => ST2 = (byte)((ST2 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool SuperTrain2_SPE { get => (ST2 & (1 << 4)) == 1 << 4; set => ST2 = (byte)((ST2 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool SuperTrain2_DEF { get => (ST2 & (1 << 5)) == 1 << 5; set => ST2 = (byte)((ST2 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool SuperTrain3_SPA { get => (ST2 & (1 << 6)) == 1 << 6; set => ST2 = (byte)((ST2 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool SuperTrain3_HP { get => (ST2 & (1 << 7)) == 1 << 7; set => ST2 = (byte)((ST2 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        private byte ST3 { get => Data[0x2E]; set => Data[0x2E] = value; }
        public bool SuperTrain3_ATK { get => (ST3 & (1 << 0)) == 1 << 0; set => ST3 = (byte)((ST3 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool SuperTrain3_SPD { get => (ST3 & (1 << 1)) == 1 << 1; set => ST3 = (byte)((ST3 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool SuperTrain3_SPE { get => (ST3 & (1 << 2)) == 1 << 2; set => ST3 = (byte)((ST3 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool SuperTrain3_DEF { get => (ST3 & (1 << 3)) == 1 << 3; set => ST3 = (byte)((ST3 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool SuperTrain4_1 { get => (ST3 & (1 << 4)) == 1 << 4; set => ST3 = (byte)((ST3 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool SuperTrain5_1 { get => (ST3 & (1 << 5)) == 1 << 5; set => ST3 = (byte)((ST3 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool SuperTrain5_2 { get => (ST3 & (1 << 6)) == 1 << 6; set => ST3 = (byte)((ST3 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool SuperTrain5_3 { get => (ST3 & (1 << 7)) == 1 << 7; set => ST3 = (byte)((ST3 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        private byte ST4 { get => Data[0x2F]; set => Data[0x2F] = value; }
        public bool SuperTrain5_4 { get => (ST4 & (1 << 0)) == 1 << 0; set => ST4 = (byte)((ST4 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool SuperTrain6_1 { get => (ST4 & (1 << 1)) == 1 << 1; set => ST4 = (byte)((ST4 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool SuperTrain6_2 { get => (ST4 & (1 << 2)) == 1 << 2; set => ST4 = (byte)((ST4 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool SuperTrain6_3 { get => (ST4 & (1 << 3)) == 1 << 3; set => ST4 = (byte)((ST4 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool SuperTrain7_1 { get => (ST4 & (1 << 4)) == 1 << 4; set => ST4 = (byte)((ST4 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool SuperTrain7_2 { get => (ST4 & (1 << 5)) == 1 << 5; set => ST4 = (byte)((ST4 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool SuperTrain7_3 { get => (ST4 & (1 << 6)) == 1 << 6; set => ST4 = (byte)((ST4 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool SuperTrain8_1 { get => (ST4 & (1 << 7)) == 1 << 7; set => ST4 = (byte)((ST4 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public uint SuperTrainBitFlags { get => BitConverter.ToUInt32(Data, 0x2C); set => BitConverter.GetBytes(value).CopyTo(Data); }
        private byte RIB0 { get => Data[0x30]; set => Data[0x30] = value; } // Ribbons are read as uints, but let's keep them per byte.
        private byte RIB1 { get => Data[0x31]; set => Data[0x31] = value; }
        private byte RIB2 { get => Data[0x32]; set => Data[0x32] = value; }
        private byte RIB3 { get => Data[0x33]; set => Data[0x33] = value; }
        private byte RIB4 { get => Data[0x34]; set => Data[0x34] = value; }
        private byte RIB5 { get => Data[0x35]; set => Data[0x35] = value; }
        private byte RIB6 { get => Data[0x36]; set => Data[0x36] = value; }
        //private byte RIB7 { get => Data[0x37]; set => Data[0x37] = value; } // Unused
        public bool RibbonChampionKalos { get => (RIB0 & (1 << 0)) == 1 << 0; set => RIB0 = (byte)((RIB0 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonChampionG3Hoenn { get => (RIB0 & (1 << 1)) == 1 << 1; set => RIB0 = (byte)((RIB0 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RibbonChampionSinnoh { get => (RIB0 & (1 << 2)) == 1 << 2; set => RIB0 = (byte)((RIB0 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool RibbonBestFriends { get => (RIB0 & (1 << 3)) == 1 << 3; set => RIB0 = (byte)((RIB0 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool RibbonTraining { get => (RIB0 & (1 << 4)) == 1 << 4; set => RIB0 = (byte)((RIB0 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool RibbonBattlerSkillful { get => (RIB0 & (1 << 5)) == 1 << 5; set => RIB0 = (byte)((RIB0 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool RibbonBattlerExpert { get => (RIB0 & (1 << 6)) == 1 << 6; set => RIB0 = (byte)((RIB0 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool RibbonEffort { get => (RIB0 & (1 << 7)) == 1 << 7; set => RIB0 = (byte)((RIB0 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public bool RibbonAlert { get => (RIB1 & (1 << 0)) == 1 << 0; set => RIB1 = (byte)((RIB1 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonShock { get => (RIB1 & (1 << 1)) == 1 << 1; set => RIB1 = (byte)((RIB1 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RibbonDowncast { get => (RIB1 & (1 << 2)) == 1 << 2; set => RIB1 = (byte)((RIB1 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool RibbonCareless { get => (RIB1 & (1 << 3)) == 1 << 3; set => RIB1 = (byte)((RIB1 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool RibbonRelax { get => (RIB1 & (1 << 4)) == 1 << 4; set => RIB1 = (byte)((RIB1 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool RibbonSnooze { get => (RIB1 & (1 << 5)) == 1 << 5; set => RIB1 = (byte)((RIB1 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool RibbonSmile { get => (RIB1 & (1 << 6)) == 1 << 6; set => RIB1 = (byte)((RIB1 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool RibbonGorgeous { get => (RIB1 & (1 << 7)) == 1 << 7; set => RIB1 = (byte)((RIB1 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public bool RibbonRoyal { get => (RIB2 & (1 << 0)) == 1 << 0; set => RIB2 = (byte)((RIB2 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonGorgeousRoyal { get => (RIB2 & (1 << 1)) == 1 << 1; set => RIB2 = (byte)((RIB2 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RibbonArtist { get => (RIB2 & (1 << 2)) == 1 << 2; set => RIB2 = (byte)((RIB2 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool RibbonFootprint { get => (RIB2 & (1 << 3)) == 1 << 3; set => RIB2 = (byte)((RIB2 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool RibbonRecord { get => (RIB2 & (1 << 4)) == 1 << 4; set => RIB2 = (byte)((RIB2 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool RibbonLegend { get => (RIB2 & (1 << 5)) == 1 << 5; set => RIB2 = (byte)((RIB2 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool RibbonCountry { get => (RIB2 & (1 << 6)) == 1 << 6; set => RIB2 = (byte)((RIB2 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool RibbonNational { get => (RIB2 & (1 << 7)) == 1 << 7; set => RIB2 = (byte)((RIB2 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public bool RibbonEarth { get => (RIB3 & (1 << 0)) == 1 << 0; set => RIB3 = (byte)((RIB3 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonWorld { get => (RIB3 & (1 << 1)) == 1 << 1; set => RIB3 = (byte)((RIB3 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RibbonClassic { get => (RIB3 & (1 << 2)) == 1 << 2; set => RIB3 = (byte)((RIB3 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool RibbonPremier { get => (RIB3 & (1 << 3)) == 1 << 3; set => RIB3 = (byte)((RIB3 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool RibbonEvent { get => (RIB3 & (1 << 4)) == 1 << 4; set => RIB3 = (byte)((RIB3 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool RibbonBirthday { get => (RIB3 & (1 << 5)) == 1 << 5; set => RIB3 = (byte)((RIB3 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool RibbonSpecial { get => (RIB3 & (1 << 6)) == 1 << 6; set => RIB3 = (byte)((RIB3 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool RibbonSouvenir { get => (RIB3 & (1 << 7)) == 1 << 7; set => RIB3 = (byte)((RIB3 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public bool RibbonWishing { get => (RIB4 & (1 << 0)) == 1 << 0; set => RIB4 = (byte)((RIB4 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonChampionBattle { get => (RIB4 & (1 << 1)) == 1 << 1; set => RIB4 = (byte)((RIB4 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RibbonChampionRegional { get => (RIB4 & (1 << 2)) == 1 << 2; set => RIB4 = (byte)((RIB4 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool RibbonChampionNational { get => (RIB4 & (1 << 3)) == 1 << 3; set => RIB4 = (byte)((RIB4 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool RibbonChampionWorld { get => (RIB4 & (1 << 4)) == 1 << 4; set => RIB4 = (byte)((RIB4 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool RIB4_5 { get => (RIB4 & (1 << 5)) == 1 << 5; set => RIB4 = (byte)((RIB4 & ~(1 << 5)) | (value ? 1 << 5 : 0)); } // Unused
        public bool RIB4_6 { get => (RIB4 & (1 << 6)) == 1 << 6; set => RIB4 = (byte)((RIB4 & ~(1 << 6)) | (value ? 1 << 6 : 0)); } // Unused
        public bool RibbonChampionG6Hoenn { get => (RIB4 & (1 << 7)) == 1 << 7; set => RIB4 = (byte)((RIB4 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public bool RibbonContestStar { get => (RIB5 & (1 << 0)) == 1 << 0; set => RIB5 = (byte)((RIB5 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonMasterCoolness { get => (RIB5 & (1 << 1)) == 1 << 1; set => RIB5 = (byte)((RIB5 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RibbonMasterBeauty { get => (RIB5 & (1 << 2)) == 1 << 2; set => RIB5 = (byte)((RIB5 & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool RibbonMasterCuteness { get => (RIB5 & (1 << 3)) == 1 << 3; set => RIB5 = (byte)((RIB5 & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool RibbonMasterCleverness { get => (RIB5 & (1 << 4)) == 1 << 4; set => RIB5 = (byte)((RIB5 & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool RibbonMasterToughness { get => (RIB5 & (1 << 5)) == 1 << 5; set => RIB5 = (byte)((RIB5 & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool RibbonChampionAlola { get => (RIB5 & (1 << 6)) == 1 << 6; set => RIB5 = (byte)((RIB5 & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool RibbonBattleRoyale { get => (RIB5 & (1 << 7)) == 1 << 7; set => RIB5 = (byte)((RIB5 & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public bool RibbonBattleTreeGreat { get => (RIB6 & (1 << 0)) == 1 << 0; set => RIB6 = (byte)((RIB6 & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool RibbonBattleTreeMaster { get => (RIB6 & (1 << 1)) == 1 << 1; set => RIB6 = (byte)((RIB6 & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool RIB6_2 { get => (RIB6 & (1 << 2)) == 1 << 2; set => RIB6 = (byte)((RIB6 & ~(1 << 2)) | (value ? 1 << 2 : 0)); } // Unused
        public bool RIB6_3 { get => (RIB6 & (1 << 3)) == 1 << 3; set => RIB6 = (byte)((RIB6 & ~(1 << 3)) | (value ? 1 << 3 : 0)); } // Unused
        public bool RIB6_4 { get => (RIB6 & (1 << 4)) == 1 << 4; set => RIB6 = (byte)((RIB6 & ~(1 << 4)) | (value ? 1 << 4 : 0)); } // Unused
        public bool RIB6_5 { get => (RIB6 & (1 << 5)) == 1 << 5; set => RIB6 = (byte)((RIB6 & ~(1 << 5)) | (value ? 1 << 5 : 0)); } // Unused
        public bool RIB6_6 { get => (RIB6 & (1 << 6)) == 1 << 6; set => RIB6 = (byte)((RIB6 & ~(1 << 6)) | (value ? 1 << 6 : 0)); } // Unused
        public bool RIB6_7 { get => (RIB6 & (1 << 7)) == 1 << 7; set => RIB6 = (byte)((RIB6 & ~(1 << 7)) | (value ? 1 << 7 : 0)); } // Unused
        public int RibbonCountMemoryContest { get => Data[0x38]; set => Data[0x38] = (byte)value; }
        public int RibbonCountMemoryBattle { get => Data[0x39]; set => Data[0x39] = (byte)value; }
        private ushort DistByte { get => BitConverter.ToUInt16(Data, 0x3A); set => BitConverter.GetBytes(value).CopyTo(Data, 0x3A); }
        public bool DistSuperTrain1 { get => (DistByte & (1 << 0)) == 1 << 0; set => DistByte = (byte)((DistByte & ~(1 << 0)) | (value ? 1 << 0 : 0)); }
        public bool DistSuperTrain2 { get => (DistByte & (1 << 1)) == 1 << 1; set => DistByte = (byte)((DistByte & ~(1 << 1)) | (value ? 1 << 1 : 0)); }
        public bool DistSuperTrain3 { get => (DistByte & (1 << 2)) == 1 << 2; set => DistByte = (byte)((DistByte & ~(1 << 2)) | (value ? 1 << 2 : 0)); }
        public bool DistSuperTrain4 { get => (DistByte & (1 << 3)) == 1 << 3; set => DistByte = (byte)((DistByte & ~(1 << 3)) | (value ? 1 << 3 : 0)); }
        public bool DistSuperTrain5 { get => (DistByte & (1 << 4)) == 1 << 4; set => DistByte = (byte)((DistByte & ~(1 << 4)) | (value ? 1 << 4 : 0)); }
        public bool DistSuperTrain6 { get => (DistByte & (1 << 5)) == 1 << 5; set => DistByte = (byte)((DistByte & ~(1 << 5)) | (value ? 1 << 5 : 0)); }
        public bool Dist7 { get => (DistByte & (1 << 6)) == 1 << 6; set => DistByte = (byte)((DistByte & ~(1 << 6)) | (value ? 1 << 6 : 0)); }
        public bool Dist8 { get => (DistByte & (1 << 7)) == 1 << 7; set => DistByte = (byte)((DistByte & ~(1 << 7)) | (value ? 1 << 7 : 0)); }
        public uint FormDuration { get => BitConverter.ToUInt32(Data, 0x3C); set => BitConverter.GetBytes(value).CopyTo(Data, 0x3C); }
        #endregion
        #region Block B
        public override string Nickname
        {
            get => GetString(0x40, 24);
            set
            {
                if (!IsNicknamed)
                {
                    int lang = SpeciesName.GetSpeciesNameLanguage(Species, Language, value, 8);
                    if (lang == (int)LanguageID.ChineseS || lang == (int)LanguageID.ChineseT)
                    {
                        StringConverter.SetString7(value, 12, lang, chinese: true).CopyTo(Data, 0x40);
                        return;
                    }
                }
                SetString(value, 12).CopyTo(Data, 0x40);
            }
        }

        public override int Move1
        {
            get => BitConverter.ToUInt16(Data, 0x5A);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x5A);
        }

        public override int Move2
        {
            get => BitConverter.ToUInt16(Data, 0x5C);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x5C);
        }

        public override int Move3
        {
            get => BitConverter.ToUInt16(Data, 0x5E);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x5E);
        }

        public override int Move4
        {
            get => BitConverter.ToUInt16(Data, 0x60);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x60);
        }

        public override int Move1_PP { get => Data[0x62]; set => Data[0x62] = (byte)value; }
        public override int Move2_PP { get => Data[0x63]; set => Data[0x63] = (byte)value; }
        public override int Move3_PP { get => Data[0x64]; set => Data[0x64] = (byte)value; }
        public override int Move4_PP { get => Data[0x65]; set => Data[0x65] = (byte)value; }
        public override int Move1_PPUps { get => Data[0x66]; set => Data[0x66] = (byte)value; }
        public override int Move2_PPUps { get => Data[0x67]; set => Data[0x67] = (byte)value; }
        public override int Move3_PPUps { get => Data[0x68]; set => Data[0x68] = (byte)value; }
        public override int Move4_PPUps { get => Data[0x69]; set => Data[0x69] = (byte)value; }

        public override int RelearnMove1
        {
            get => BitConverter.ToUInt16(Data, 0x6A);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x6A);
        }

        public override int RelearnMove2
        {
            get => BitConverter.ToUInt16(Data, 0x6C);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x6C);
        }

        public override int RelearnMove3
        {
            get => BitConverter.ToUInt16(Data, 0x6E);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x6E);
        }

        public override int RelearnMove4
        {
            get => BitConverter.ToUInt16(Data, 0x70);
            set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0x70);
        }

        public bool SecretSuperTrainingUnlocked { get => (Data[0x72] & 1) == 1; set => Data[0x72] = (byte)((Data[0x72] & ~1) | (value ? 1 : 0)); }
        public bool SecretSuperTrainingComplete { get => (Data[0x72] & 2) == 2; set => Data[0x72] = (byte)((Data[0x72] & ~2) | (value ? 2 : 0)); }
        public byte _0x73 { get => Data[0x73]; set => Data[0x73] = value; }
        private uint IV32 { get => BitConverter.ToUInt32(Data, 0x74); set => BitConverter.GetBytes(value).CopyTo(Data, 0x74); }
        public override int IV_HP { get => (int)(IV32 >> 00) & 0x1F; set => IV32 = (IV32 & ~(0x1Fu << 00)) | ((value > 31 ? 31u : (uint)value) << 00); }
        public override int IV_ATK { get => (int)(IV32 >> 05) & 0x1F; set => IV32 = (IV32 & ~(0x1Fu << 05)) | ((value > 31 ? 31u : (uint)value) << 05); }
        public override int IV_DEF { get => (int)(IV32 >> 10) & 0x1F; set => IV32 = (IV32 & ~(0x1Fu << 10)) | ((value > 31 ? 31u : (uint)value) << 10); }
        public override int IV_SPE { get => (int)(IV32 >> 15) & 0x1F; set => IV32 = (IV32 & ~(0x1Fu << 15)) | ((value > 31 ? 31u : (uint)value) << 15); }
        public override int IV_SPA { get => (int)(IV32 >> 20) & 0x1F; set => IV32 = (IV32 & ~(0x1Fu << 20)) | ((value > 31 ? 31u : (uint)value) << 20); }
        public override int IV_SPD { get => (int)(IV32 >> 25) & 0x1F; set => IV32 = (IV32 & ~(0x1Fu << 25)) | ((value > 31 ? 31u : (uint)value) << 25); }
        public override bool IsEgg { get => ((IV32 >> 30) & 1) == 1; set => IV32 = (IV32 & ~0x40000000u) | (value ? 0x40000000u : 0u); }
        public override bool IsNicknamed { get => ((IV32 >> 31) & 1) == 1; set => IV32 = (IV32 & 0x7FFFFFFFu) | (value ? 0x80000000u : 0u); }
        #endregion
        #region Block C
        public override string HT_Name { get => GetString(0x78, 24); set => SetString(value, 12).CopyTo(Data, 0x78); }
        public override int HT_Gender { get => Data[0x92]; set => Data[0x92] = (byte)value; }
        public override int CurrentHandler { get => Data[0x93]; set => Data[0x93] = (byte)value; }
        public int Geo1_Region { get => Data[0x94]; set => Data[0x94] = (byte)value; }
        public int Geo1_Country { get => Data[0x95]; set => Data[0x95] = (byte)value; }
        public int Geo2_Region { get => Data[0x96]; set => Data[0x96] = (byte)value; }
        public int Geo2_Country { get => Data[0x97]; set => Data[0x97] = (byte)value; }
        public int Geo3_Region { get => Data[0x98]; set => Data[0x98] = (byte)value; }
        public int Geo3_Country { get => Data[0x99]; set => Data[0x99] = (byte)value; }
        public int Geo4_Region { get => Data[0x9A]; set => Data[0x9A] = (byte)value; }
        public int Geo4_Country { get => Data[0x9B]; set => Data[0x9B] = (byte)value; }
        public int Geo5_Region { get => Data[0x9C]; set => Data[0x9C] = (byte)value; }
        public int Geo5_Country { get => Data[0x9D]; set => Data[0x9D] = (byte)value; }
        public byte _0x9E { get => Data[0x9E]; set => Data[0x9E] = value; }
        public byte _0x9F { get => Data[0x9F]; set => Data[0x9F] = value; }
        public byte _0xA0 { get => Data[0xA0]; set => Data[0xA0] = value; }
        public byte _0xA1 { get => Data[0xA1]; set => Data[0xA1] = value; }
        public override int HT_Friendship { get => Data[0xA2]; set => Data[0xA2] = (byte)value; }
        public override int HT_Affection { get => Data[0xA3]; set => Data[0xA3] = (byte)value; }
        public override int HT_Intensity { get => Data[0xA4]; set => Data[0xA4] = (byte)value; }
        public override int HT_Memory { get => Data[0xA5]; set => Data[0xA5] = (byte)value; }
        public override int HT_Feeling { get => Data[0xA6]; set => Data[0xA6] = (byte)value; }
        public byte _0xA7 { get => Data[0xA7]; set => Data[0xA7] = value; }
        public override int HT_TextVar { get => BitConverter.ToUInt16(Data, 0xA8); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xA8); }
        public byte _0xAA { get => Data[0xAA]; set => Data[0xAA] = value; }
        public byte _0xAB { get => Data[0xAB]; set => Data[0xAB] = value; }
        public byte _0xAC { get => Data[0xAC]; set => Data[0xAC] = value; }
        public byte _0xAD { get => Data[0xAD]; set => Data[0xAD] = value; }
        public override byte Fullness { get => Data[0xAE]; set => Data[0xAE] = value; }
        public override byte Enjoyment { get => Data[0xAF]; set => Data[0xAF] = value; }
        #endregion
        #region Block D
        public override string OT_Name { get => GetString(0xB0, 24); set => SetString(value, 12).CopyTo(Data, 0xB0); }
        public override int OT_Friendship { get => Data[0xCA]; set => Data[0xCA] = (byte)value; }
        public override int OT_Affection { get => Data[0xCB]; set => Data[0xCB] = (byte)value; }
        public override int OT_Intensity { get => Data[0xCC]; set => Data[0xCC] = (byte)value; }
        public override int OT_Memory { get => Data[0xCD]; set => Data[0xCD] = (byte)value; }
        public override int OT_TextVar { get => BitConverter.ToUInt16(Data, 0xCE); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xCE); }
        public override int OT_Feeling { get => Data[0xD0]; set => Data[0xD0] = (byte)value; }
        public override int Egg_Year { get => Data[0xD1]; set => Data[0xD1] = (byte)value; }
        public override int Egg_Month { get => Data[0xD2]; set => Data[0xD2] = (byte)value; }
        public override int Egg_Day { get => Data[0xD3]; set => Data[0xD3] = (byte)value; }
        public override int Met_Year { get => Data[0xD4]; set => Data[0xD4] = (byte)value; }
        public override int Met_Month { get => Data[0xD5]; set => Data[0xD5] = (byte)value; }
        public override int Met_Day { get => Data[0xD6]; set => Data[0xD6] = (byte)value; }
        public byte _0xD7 { get => Data[0xD7]; set => Data[0xD7] = value; }
        public override int Egg_Location { get => BitConverter.ToUInt16(Data, 0xD8); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xD8); }
        public override int Met_Location { get => BitConverter.ToUInt16(Data, 0xDA); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xDA); }
        public override int Ball { get => Data[0xDC]; set => Data[0xDC] = (byte)value; }
        public override int Met_Level { get => Data[0xDD] & ~0x80; set => Data[0xDD] = (byte)((Data[0xDD] & 0x80) | value); }
        public override int OT_Gender { get => Data[0xDD] >> 7; set => Data[0xDD] = (byte)((Data[0xDD] & ~0x80) | (value << 7)); }
        public int HyperTrainFlags { get => Data[0xDE]; set => Data[0xDE] = (byte)value; }
        public bool HT_HP { get => ((HyperTrainFlags >> 0) & 1) == 1; set => HyperTrainFlags = (HyperTrainFlags & ~(1 << 0)) | ((value ? 1 : 0) << 0); }
        public bool HT_ATK { get => ((HyperTrainFlags >> 1) & 1) == 1; set => HyperTrainFlags = (HyperTrainFlags & ~(1 << 1)) | ((value ? 1 : 0) << 1); }
        public bool HT_DEF { get => ((HyperTrainFlags >> 2) & 1) == 1; set => HyperTrainFlags = (HyperTrainFlags & ~(1 << 2)) | ((value ? 1 : 0) << 2); }
        public bool HT_SPA { get => ((HyperTrainFlags >> 3) & 1) == 1; set => HyperTrainFlags = (HyperTrainFlags & ~(1 << 3)) | ((value ? 1 : 0) << 3); }
        public bool HT_SPD { get => ((HyperTrainFlags >> 4) & 1) == 1; set => HyperTrainFlags = (HyperTrainFlags & ~(1 << 4)) | ((value ? 1 : 0) << 4); }
        public bool HT_SPE { get => ((HyperTrainFlags >> 5) & 1) == 1; set => HyperTrainFlags = (HyperTrainFlags & ~(1 << 5)) | ((value ? 1 : 0) << 5); }
        public override int Version { get => Data[0xDF]; set => Data[0xDF] = (byte)value; }
        public override int Country { get => Data[0xE0]; set => Data[0xE0] = (byte)value; }
        public override int Region { get => Data[0xE1]; set => Data[0xE1] = (byte)value; }
        public override int ConsoleRegion { get => Data[0xE2]; set => Data[0xE2] = (byte)value; }
        public override int Language { get => Data[0xE3]; set => Data[0xE3] = (byte)value; }
        #endregion
        #region Battle Stats
        public override int Status_Condition { get => BitConverter.ToInt32(Data, 0xE8); set => BitConverter.GetBytes(value).CopyTo(Data, 0xE8); }
        public override int Stat_Level { get => Data[0xEC]; set => Data[0xEC] = (byte)value; }
        public byte DirtType { get => Data[0xED]; set => Data[0xED] = value; }
        public byte DirtLocation { get => Data[0xEE]; set => Data[0xEE] = value; }
        // 0xEF unused
        public override int Stat_HPCurrent { get => BitConverter.ToUInt16(Data, 0xF0); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xF0); }
        public override int Stat_HPMax { get => BitConverter.ToUInt16(Data, 0xF2); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xF2); }
        public override int Stat_ATK { get => BitConverter.ToUInt16(Data, 0xF4); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xF4); }
        public override int Stat_DEF { get => BitConverter.ToUInt16(Data, 0xF6); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xF6); }
        public override int Stat_SPE { get => BitConverter.ToUInt16(Data, 0xF8); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xF8); }
        public override int Stat_SPA { get => BitConverter.ToUInt16(Data, 0xFA); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xFA); }
        public override int Stat_SPD { get => BitConverter.ToUInt16(Data, 0xFC); set => BitConverter.GetBytes((ushort)value).CopyTo(Data, 0xFC); }
        #endregion

        public bool IsUntradedEvent6 => Geo1_Country == 0 && Geo1_Region == 0 && Met_Location / 10000 == 4 && Gen6;

        public override int[] Markings
        {
            get
            {
                int[] marks = new int[8];
                int val = MarkValue;
                for (int i = 0; i < marks.Length; i++)
                    marks[i] = ((val >> (i * 2)) & 3) % 3;
                return marks;
            }
            set
            {
                if (value.Length > 8)
                    return;
                int v = 0;
                for (int i = 0; i < value.Length; i++)
                    v |= (value[i] % 3) << (i * 2);
                MarkValue = v;
            }
        }

        public void FixMemories()
        {
            if (IsEgg) // No memories if is egg.
            {
                HT_Friendship = HT_Affection = HT_TextVar = HT_Memory = HT_Intensity = HT_Feeling =
                /* OT_Friendship */ OT_Affection = OT_TextVar = OT_Memory = OT_Intensity = OT_Feeling = 0;
                this.ClearGeoLocationData();

                // Clear Handler
                HT_Name = string.Empty.PadRight(11, '\0');
                return;
            }

            if (IsUntraded)
                HT_Friendship = HT_Affection = HT_TextVar = HT_Memory = HT_Intensity = HT_Feeling = 0;
            if (GenNumber < 6)
                /* OT_Affection = */
                OT_TextVar = OT_Memory = OT_Intensity = OT_Feeling = 0;

            this.SanitizeGeoLocationData();

            if (GenNumber < 7) // must be transferred via bank, and must have memories
            {
                TradeMemory(Bank: true);
                // georegions cleared on 6->7, no need to set
            }
        }

        protected override bool TradeOT(ITrainerInfo tr)
        {
            // Check to see if the OT matches the SAV's OT info.
            if (!(tr.OT == OT_Name && tr.TID == TID && tr.SID == SID && tr.Gender == OT_Gender))
                return false;

            CurrentHandler = 0;
            return true;
        }

        protected override void TradeHT(ITrainerInfo tr)
        {
            if (tr.OT != HT_Name || tr.Gender != HT_Gender || (Geo1_Country == 0 && Geo1_Region == 0 && !IsUntradedEvent6))
            {
                // No geolocations are set ingame -- except for bank transfers. Don't emulate bank transfers
                // this.TradeGeoLocation(tr.Country, tr.SubRegion);
            }

            if (HT_Name != tr.OT)
            {
                HT_Friendship = PersonalInfo.BaseFriendship;
                HT_Affection = 0;
                HT_Name = tr.OT;
            }
            CurrentHandler = 1;
            HT_Gender = tr.Gender;
        }

        // Misc Updates
        public override void TradeMemory(bool Bank)
        {
            if (Bank)
                base.TradeMemory(true);
        }

        // Maximums
        public override int MaxMoveID => Legal.MaxMoveID_8;
        public override int MaxSpeciesID => Legal.MaxSpeciesID_8;
        public override int MaxAbilityID => Legal.MaxAbilityID_8;
        public override int MaxItemID => Legal.MaxItemID_8;
        public override int MaxBallID => Legal.MaxBallID_8;
        public override int MaxGameID => Legal.MaxGameID_8;
    }
}