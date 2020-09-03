﻿namespace OpenTracker.Models.RequirementNodes
{
    /// <summary>
    /// This is the enum type for requirement node identity.
    /// </summary>
    public enum RequirementNodeID
    {
        Inaccessible,
        Start,
        EntranceShuffle,
        NonEntrance,
        NonEntranceInverted,
        LightWorld,
        LightWorldInverted,
        LightWorldNonInverted,
        LightWorldInspect,
        LightWorldMirror,
        LightWorldNotBunny,
        LightWorldNotBunnyOrDungeonRevive,
        LightWorldNotBunnyOrSuperBunnyFallInHole,
        LightWorldNotBunnyOrSuperBunnyMirror,
        LightWorldDash,
        LightWorldHammer,
        LightWorldLift1,
        GroveDiggingSpot,
        Flute,
        FluteInverted,
        FluteNonInverted,
        Pedestal,
        LumberjackCaveHole,
        LumberjackCave,
        DeathMountainEntry,
        DeathMountainEntryNonEntrance,
        DeathMountainEntryCave,
        DeathMountainEntryCaveDark,
        DeathMountainExit,
        DeathMountainExitNonEntrance,
        DeathMountainExitCave,
        DeathMountainExitCaveDark,
        ForestHideout,
        LWKakarikoPortal,
        LWKakarikoPortalNonInverted,
        LWKakarikoPortalNotBunny,
        SickKid,
        GrassHouse,
        BombHut,
        MagicBatLedge,
        MagicBat,
        MagicBatEntrance,
        Library,
        RaceGameLedge,
        RaceGame,
        SouthOfGroveLedge,
        SouthOfGrove,
        DesertLedge,
        DesertLedgeItem,
        DesertLedgeNotBunny,
        DesertBack,
        DesertBackNotBunny,
        CheckerboardLedge,
        CheckerboardLedgeNotBunny,
        CheckerboardCave,
        DesertPalaceFrontEntrance,
        BombosTabletLedge,
        BombosTablet,
        LWMirePortal,
        LWMirePortalNonInverted,
        LWGraveyard,
        LWGraveyardNotBunny,
        LWGraveyardLedge,
        EscapeGrave,
        SanctuaryGraveEntrance,
        KingsTomb,
        KingsTombNotBunny,
        KingsTombGrave,
        HoulihanHoleEntrance,
        HyruleCastleTop,
        HyruleCastleTopInverted,
        HyruleCastleTopNonInverted,
        AgahnimTowerEntrance,
        GanonHole,
        LWSouthPortal,
        LWSouthPortalNonInverted,
        LWSouthPortalNotBunny,
        ZoraArea,
        ZoraLedge,
        WaterfallFairy,
        WaterfallFairyNotBunny,
        LWWitchArea,
        LWWitchAreaNotBunny,
        WitchsHut,
        Sahasrahla,
        LWEastPortal,
        LWEastPortalNonInverted,
        LWEastPortalNotBunny,
        LWLakeHyliaFakeFlippers,
        LWLakeHyliaFlippers,
        LWLakeHyliaWaterWalk,
        Hobo,
        LakeHyliaIsland,
        LakeHyliaIslandItem,
        LakeHyliaFairyIsland,
        LakeHyliaFairyIslandNonInverted,
        DeathMountainWestBottom,
        DeathMountainWestBottomNonEntrance,
        DeathMountainWestBottomNotBunny,
        SpectacleRockTop,
        SpectacleRockTopItem,
        DeathMountainWestTop,
        DeathMountainWestTopNotBunny,
        EtherTablet,
        DeathMountainEastBottom,
        DeathMountainEastBottomNotBunny,
        DeathMountainEastBottomLift2,
        DeathMountainEastBottomConnector,
        ParadoxCave,
        ParadoxCaveNotBunny,
        DeathMountainEastTop,
        DeathMountainEastTopInverted,
        DeathMountainEastTopNotBunny,
        DeathMountainEastTopConnector,
        SpiralCaveLedge,
        SpiralCave,
        MimicCaveLedge,
        MimicCaveLedgeNotBunny,
        MimicCave,
        LWFloatingIsland,
        FloatingIsland,
        LWTurtleRockTop,
        LWTurtleRockTopInverted,
        LWTurtleRockTopInvertedNotBunny,
        LWTurtleRockTopNonInverted,
        DWKakarikoPortal,
        DWKakarikoPortalInverted,
        DarkWorldWest,
        DarkWorldWestMirror,
        DarkWorldWestNotBunny,
        DarkWorldWestNotBunnyOrSuperBunnyMirror,
        DarkWorldWestLift2,
        SkullWoodsBack,
        BumperCaveEntry,
        BumperCaveEntryNonEntrance,
        BumperCaveFront,
        BumperCaveNotBunny,
        BumperCavePastGap,
        BumperCaveBack,
        BumperCaveTop,
        BumperCaveItem,
        HammerHouse,
        HammerHouseNotBunny,
        HammerPegsArea,
        HammerPegs,
        PurpleChest,
        BlacksmithPrison,
        Blacksmith,
        DarkWorldSouth,
        DarkWorldSouthInverted,
        DarkWorldSouthNonInverted,
        DarkWorldSouthMirror,
        DarkWorldSouthNotBunny,
        DarkWorldSouthDash,
        DarkWorldSouthHammer,
        BuyBigBomb,
        BuyBigBombNonInverted,
        BigBombToLightWorld,
        BigBombToLightWorldNonInverted,
        BigBombToDWLakeHylia,
        BigBombToWall,
        DWSouthPortal,
        DWSouthPortalInverted,
        DWSouthPortalNotBunny,
        MireArea,
        MireAreaMirror,
        MireAreaNotBunny,
        MireAreaNotBunnyOrSuperBunnyMirror,
        MiseryMireEntrance,
        DWMirePortal,
        DWMirePortalInverted,
        DWGraveyard,
        DWGraveyardMirror,
        DWGraveyardLedge,
        DWWitchArea,
        DWWitchAreaNotBunny,
        CatfishArea,
        DarkWorldEast,
        DarkWorldEastNonInverted,
        DarkWorldEastMirror,
        DarkWorldEastNotBunny,
        DarkWorldEastHammer,
        DWEastPortal,
        DWEastPortalInverted,
        DWEastPortalNotBunny,
        DWLakeHyliaFlippers,
        DWLakeHyliaFakeFlippers,
        DWLakeHyliaWaterWalk,
        IcePalaceIsland,
        IcePalaceIslandInverted,
        DarkWorldSouthEast,
        DarkWorldSouthEastNotBunny,
        DarkWorldSouthEastLift1,
        DarkDeathMountainWestBottom,
        DarkDeathMountainWestBottomInverted,
        DarkDeathMountainWestBottomNonEntrance,
        DarkDeathMountainWestBottomMirror,
        DarkDeathMountainWestBottomNotBunny,
        SpikeCavePastHammerBlocks,
        SpikeCavePastSpikes,
        SpikeCaveChest,
        DarkDeathMountainTop,
        DarkDeathMountainTopInverted,
        DarkDeathMountainTopNonInverted,
        DarkDeathMountainTopMirror,
        DarkDeathMountainTopNotBunny,
        SuperBunnyCave,
        SuperBunnyCaveChests,
        GanonsTowerEntrance,
        GanonsTowerEntranceNonInverted,
        DWFloatingIsland,
        HookshotCaveEntrance,
        HookshotCaveEntranceHookshot,
        HookshotCaveEntranceHover,
        HookshotCaveBonkableChest,
        HookshotCaveBack,
        DWTurtleRockTop,
        DWTurtleRockTopInverted,
        DWTurtleRockTopNotBunny,
        TurtleRockFrontEntrance,
        DarkDeathMountainEastBottom,
        DarkDeathMountainEastBottomInverted,
        DarkDeathMountainEastBottomConnector,
        TurtleRockTunnel,
        TurtleRockTunnelMirror,
        TurtleRockSafetyDoor,
        HCSanctuaryEntry,
        HCFrontEntry,
        HCBackEntry,
        ATEntry,
        EPEntry,
        DPFrontEntry,
        DPLeftEntry,
        DPBackEntry,
        ToHEntry,
        PoDEntry,
        SPEntry,
        SWFrontEntry,
        SWBackEntry,
        TTEntry,
        IPEntry,
        MMEntry,
        TRFrontEntry,
        TRFrontEntryNonInverted,
        TRFrontEntryNonInvertedNonEntrance,
        TRFrontToKeyDoors,
        TRKeyDoorsToMiddleExit,
        TRMiddleEntry,
        TRBackEntry,
        GTEntry
    }
}
