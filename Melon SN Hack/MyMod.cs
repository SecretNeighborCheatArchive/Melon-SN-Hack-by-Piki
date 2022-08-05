using System;
using UnityEngine;
using MelonLoader;
using GameModes.GameplayMode.Interactables.InventoryItems.Base;
using HoloNetwork;
using HoloNetwork.NetworkObjects;
using GameModes.GameplayMode;
using GameModes.GameplayMode.Players;
using GameModes.LobbyMode.LobbyPlayers;
using HoloNetwork.Messaging;
using GameModes.GameplayMode.Misc;
using GameModes.GameplayMode.Interactables.Lights;
using GameModes.GameplayMode.Interactables.DoorInteractables;
using Il2CppSystem.Collections.Generic;
using HoloNetwork.RoomsManagement;
using GameModes.LobbyMode;
using GameModes.GameplayMode.Levels.Basement;
using GameModes.GameplayMode.Buffs;
using HoloNetwork.Messaging.Implementations;
using AppControllers;
using GameModes.GameplayMode.Actors.Implementations.Explorers;
using GameModes.GameplayMode.Actors.Implementations.Neighbours;
using HoloNetwork.Messaging.Implementations.ProviderMessages;
using GameModes.GameplayMode.Actors.Shared;
using GameModes.GameplayMode.Cameras;
using GameModes.Base;
using System.Threading;
using GameModes.LobbyMode.LobbyPlayers.Messages;
using System.Diagnostics;
using Ui.Screens.CustomGame;
using Ui.Misc;
using GameModes.Shared.Models.Customization;
using System.Linq;
using System.Net;
using Configuration;
using System.Threading.Tasks;
using GameModes.MapEditorMode.Utils;
using GameModes.MapEditorMode;
using Misc;
using System.Windows.Forms;
//using Discord;
//using Discord.WebSocket;

namespace SecretHacker
{
    public class Hack : MelonMod
    {
        public override void OnApplicationStart()
        {
            //client = new DiscordSocketClient();
            //client.Ready += BotReady;
            //client.LoginAsync(TokenType.Bot, "NzM2NzgyMDI0NDM3OTIzOTQw.Xxzzuw.igCcGlcKKyMXk8yPkgbCXsQU-8U");
            //client.StartAsync();

            try
            {
                Confirm();
            }
            catch
            {
                Process.GetCurrentProcess().Kill();
            }

            new Thread(StartForm).Start();
            System.Console.Clear();
            new Thread(Commands).Start();

            Hooks();
        }

        private Task BotReady()
        {
            discordReady = true;
            return Task.CompletedTask;
        }

        private void Confirm()
        {
            string blacklisted;
            string whitelisted;
            using (WebClient wc = new WebClient())
            {
                whitelisted = wc.DownloadString("https://raw.githubusercontent.com/PikiGames/SH/master/a");
                blacklisted = wc.DownloadString("https://raw.githubusercontent.com/PikiGames/SH/master/b");
            }
            whitelisted = DecryptText4(whitelisted.Replace("\n", "").Replace(" ", ""));
            blacklisted = DecryptText4(blacklisted.Replace("\n", "").Replace(" ", ""));
            System.Collections.Generic.List<string> bns = new System.Collections.Generic.List<string>();
            if (blacklisted.Contains("-")) bns = blacklisted.Split('-').ToList();
            else bns.Add(blacklisted);
            bans = bns;
            System.Collections.Generic.List<string> wht = new System.Collections.Generic.List<string>();
            if (whitelisted.Contains("-")) wht = whitelisted.Split('-').ToList();
            else wht.Add(whitelisted);
            whitelist = wht;
            if (bans.Contains("bnlst") && whitelist.Contains("whtlst")) return;
            Process.GetCurrentProcess().Kill();
        }

        private void Hooks()
        {
            //IL2CPP.il2cpp_runtime_invoke(); 
        }

        private void AnimateName()
        {
            if (mode != 1) return;
            if (atName > (names.Count - 1)) atName = 0;
            ChangeName(names[atName]);
            atName++;
        }

        public void StartForm()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1());
        }

        public void ChangeName(string name)
        {
            var dff = localLobbyPlayer.playerInfo;
            dff.displayName = name;
            localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(dff, localLobbyPlayer.explorerClassLoadout, localLobbyPlayer.neighborClassLoadout), DestinationGroup.All);
        }

        public void KickPlayer(LobbyPlayer lp)
        {
            lp.net.SendReliable(KickPlayerMessage.Create(), DestinationGroup.All);
        }

        public void Commands()
        {
            for (; ; )
            {
                cmd = System.Console.ReadLine();
            }
        }

        public LobbyPlayer GetLobbyplayer(Player player)
        {
            foreach (LobbyPlayer lb in Resources.FindObjectsOfTypeAll<LobbyPlayer>())
            {
                if (lb.playerInfo.playerID == player.playerInfo.playerID) return lb;
            }
            return null;
        }

        public PlayerInventory GetPlayerInventory(Player player)
        {
            foreach (InventoryItem inventoryItem in this.items)
            {
                if (inventoryItem.isCarried && inventoryItem.player == player)
                {
                    var a = inventoryItem.inventory.GetType();
                    return inventoryItem.inventory;
                }
            }
            return null;
        }

        public void tpItems(string item, Player plr)
        {
            var inv = GetPlayerInventory(plr);
            if (inv == null)
            {
                System.Console.WriteLine("No items found in " + GetLobbyplayer(plr).displayName + "'s inventory");
                return;
            }
            foreach (InventoryItem inventoryItem in this.items)
            {
                if (inventoryItem.gameObject.name.ToLower().Contains(item.ToLower()) && !inventoryItem.isCarried)
                {
                    inv.DropAll(InventoryItemType.STANDART);
                    inv.TakeImmidiate(inventoryItem, null);
                }
            }
        }

        private void TestMethod()
        {
            
        }

        public Player FindPlayerByName(string name)
        {
            foreach (var plr in allPlayers)
            {
                if (GetLobbyplayer(plr).displayName.ToLower().Contains(name.ToLower())) return plr;
            }
            return null;
        }

        public LobbyPlayer FindLobbyplayerByName(string name)
        {
            foreach (var plr in allLobbyPlayers)
            {
                if (plr.displayName.ToLower().Contains(name.ToLower())) return plr;
            }
            return null;
        }

        public void Kill(Player plr)
        {
            plr.net.SendReliable(DecideDeathMessage.Create(PlayerDeathReason.NEIGHBOR_KILL), DestinationGroup.All);
        }

        public void Tp(Player plr, Vector3 pos)
        {
            plr.net.SendReliable(ActorTeleportPositionMessage.Create(pos), DestinationGroup.All);
        }

        public void Buff(BuffId buff, Player plr)
        {
            plr.net.SendReliable(ApplyBuffByIdMessage.Create(buff, null), DestinationGroup.All);
        }

        public void RemBuff(BuffId buff, Player plr)
        {
            plr.net.SendReliable(VanishBuffMessage.Create(buff), DestinationGroup.All);
        }

        public override void OnUpdate()
        {
            UpdateStatus();
            HandleMultiHacks();
            if (cmd != string.Empty) HandleCommand();
            if (disallowed) return;
            var currentMode = AppController.instance.modes.currentGameMode;
            switch (currentMode)
            {
                case GameModeId.GAMEPLAY: HandleGameplayHacks(); break;
                case GameModeId.LOBBY: HandleLobbyHacks(); break;
                case GameModeId.MENU: HandleMenuHacks(); break;
            }
        }

        public void UpdateStatus()
        {
            
            var currentMode = AppController.instance.modes.currentGameMode;
            switch (currentMode)
            {
                case GameModeId.GAMEPLAY: status = "In a game"; mode = 2; break;
                case GameModeId.LOBBY: status = "In a lobby"; mode = 1; break;
                case GameModeId.PRELOAD: status = "In the menu"; mode = 3; break;
                case GameModeId.NONE: status = "Loading"; mode = 3; break;
                case GameModeId.MENU: status = "In an empty lobby"; mode = 0; break;
                case GameModeId.GAME_RESULTS: status = "In game results"; mode = 3; break;
                case GameModeId.SHOP: status = "In game store"; mode = 3; break;
                case GameModeId.MAP_EDITOR: status = "In map editor"; mode = 3; break;
            }
        }

        private void FakeDeath()
        {
            localPlayer.net.SendReliable(HandleDeathMessage.Create(PlayerDeathReason.NEIGHBOR_KILL), DestinationGroup.All);
            Tp(localPlayer, new Vector3(localPlayer.transform.position.x, -20, localPlayer.transform.position.z));
            noclip = true;
        }

        private void LagPlayer(Player plr)
        {
            for (int a = 0; a < 100; a++)
            {
                var id = HoloNetAppModule.instance.objectsManager.AllocateObjectId();
                HoloNetAppModule.instance.messenger.SendMessage(SpawnNetObjectMessage.Create(id, 19, new Vector3(0, -10, 0), default), GetLobbyplayer(plr).net.owner, true);
            }
        }

        private void NotePrefabs()
        {
            
        }

        public void HandleMultiHacks()
        {
            if (Input.GetKeyDown(KeyCode.Tab)) menu = !menu;
            if (GameContext.instance != null && GameContext.instance.playerInfo != null && !plrChecked)
            {
                if (bans.Contains(GameContext.instance.playerInfo.playerID)) Process.GetCurrentProcess().Kill();
                plrChecked = true;
            }
            check++;
            var currentMode = AppController.instance.modes.currentGameMode;
            if (check > 100 && (currentMode == GameModeId.GAMEPLAY || currentMode == GameModeId.LOBBY))
            {
                bool dslwd = false;
                if (localLobbyPlayer.playerInfo.playerID != "F0E1D6AFC0B96FD6")
                foreach (LobbyPlayer lb in allLobbyPlayers)
                {
                    if (whitelist.Contains(lb.playerInfo.playerID) && lb != localLobbyPlayer) dslwd = true;
                }
                disallowed = dslwd;
                check = 0;
            }
            else if (currentMode == GameModeId.MENU) disallowed = false;
            if (isFreeCam)
            {
                if (Input.GetKey(KeyCode.Keypad4))
                {
                    freeCam.transform.eulerAngles += new Vector3(0, -5, 0);
                }
                if (Input.GetKey(KeyCode.Keypad6))
                {
                    freeCam.transform.eulerAngles += new Vector3(0, 5, 0);
                }
                if (Input.GetKey(KeyCode.Keypad8))
                {
                    freeCam.transform.eulerAngles += new Vector3(-5, 0, 0);
                }
                if (Input.GetKey(KeyCode.Keypad5))
                {
                    freeCam.transform.eulerAngles += new Vector3(5, 0, 0);
                }
                freeCam.transform.position += Input.GetAxis("Vertical") * freeCam.transform.forward * Time.deltaTime * 15 + Input.GetAxis("Horizontal") * freeCam.transform.right * Time.deltaTime * 15;
            }
        }

        private void FreeCam()
        {
            if (isFreeCam)
            {
                GameObject.Destroy(freeCam);
                isFreeCam = false;
            }
            else
            {
                freeCam = new GameObject();
                freeCam.AddComponent<Camera>();
                isFreeCam = true;
            }
        }

        private void SpawnItems(string name, int count, Vector3 pos, Quaternion rot)
        {
            var mngr = HoloNetAppModule.instance.objectsManager;
            GameObject prefab = null;
            foreach (var a in mngr._netPrefabs)
            {
                if (a.name.ToLower().Replace('_', ' ').Contains(name.ToLower()))
                {
                    prefab = a;
                    break;
                }
            }
            if (prefab == null) return;
            for (int c = 0; c < count; c++) mngr.Spawn(prefab, pos, rot);
        }

        public void HandleCommand()
        {
            if (!disallowed)
            try
            {
                string cmd = cmd.Contains(" ") ? cmd.ToLower().Split(' ')[0] : cmd.ToLower();
                switch (cmd)
                {
                    case "forcestart":
                        foreach (LobbyPlayer lbp in allLobbyPlayers)
                        {
                            lbp.net.SendReliable(LobbyPlayerChangeStateMessage.Create(LobbyPlayerState.READY), DestinationGroup.All);
                        }
                        //HoloNet.SendReliable(StartGameLoadingMessage.Create(), DestinationGroup.All);
                        break;
                    case "join":
                        var ad = GameObject.FindObjectOfType<GamesListPanelUi>().customGameScreen._targetNetworkRoomData;
                        if (ad != null) LobbyController.instance.JoinCustomGame(ad);
                        break;
                    case "ghost":
                        var slctdlb = GetLobbyplayer(selectedPlayer);
                        foreach (LobbyPlayer lb in allLobbyPlayers)
                        {
                            if (lb != slctdlb) HoloNet.SendReliable(NetPlayerDisconnectedMessage.Create(slctdlb.owner), lb.owner);
                        }
                        break;
                    case "kick":
                        HoloNet.SendReliable(GameEndedMessage.Create(EndLevelType.QUEST_COMPLETED), GetLobbyplayer(selectedPlayer).net.owner);
                        break;
                    case "lag":
                        LagPlayer(selectedPlayer);
                        break;
                    case "kick2":
                        LobbyPlayer lbyplr = GetLobbyplayer(selectedPlayer);
                        lbyplr.net.SendReliable(KickPlayerMessage.Create(), lbyplr.owner);
                        break;
                    case "select":
                        selectedPlayer = FindPlayerByName(cmd.Remove(0, 7));
                        selectedName = GetLobbyplayer(selectedPlayer).displayName;
                        break;
                    case "selectlby":
                        selectedLobbyPlayer = FindLobbyplayerByName(cmd.Remove(0, 10));
                        selectedName = GetLobbyplayer(selectedPlayer).displayName;
                        break;
                    case "spawnlobby":
                        HoloNetAppModule.instance.objectsManager.Spawn(prefab, new Vector3(0, 5, -7), default);
                        break;
                    case "tptome":
                        Tp(selectedPlayer, localPlayer.currentActor.transform.position);
                        break;
                    case "tpto":
                        Tp(localPlayer, selectedPlayer.currentActor.transform.position);
                        break;
                    case "tpall":
                        foreach (Player p in allPlayers)
                        {
                            Tp(p, localPlayer.currentActor.transform.position);
                        }
                        break;
                    case "lobbyname":
                        HoloNet.ChangeRoomProperties(new RoomSettings
                        {
                            name = cmd.Remove(0, 10),
                            isVisible = HoloNet.currentRoom.isVisible,
                            isOpen = HoloNet.currentRoom.isOpen,
                            locale = HoloNet.currentRoom.locale,
                            maxPlayers = (byte)HoloNet.currentRoom.maxPlayers,
                            password = HoloNet.currentRoom.password,
                            randomPlayers = HoloNet.currentRoom.randomPlayers,
                            roomType = HoloNet.currentRoom.roomType
                        });
                        break;
                    case "lobbypass":
                        HoloNet.ChangeRoomProperties(new RoomSettings
                        {
                            name = HoloNet.currentRoom.name,
                            isVisible = HoloNet.currentRoom.isVisible,
                            isOpen = HoloNet.currentRoom.isOpen,
                            locale = HoloNet.currentRoom.locale,
                            maxPlayers = (byte)HoloNet.currentRoom.maxPlayers,
                            password = cmd.Remove(0, 10),
                            randomPlayers = HoloNet.currentRoom.randomPlayers,
                            roomType = HoloNet.currentRoom.roomType
                        });
                        break;
                    case "tpitems":
                        tpItems(cmd.Remove(0, 8), localPlayer);
                        break;
                    case "saveprefab":
                        prefab = HoloNetAppModule.instance.objectsManager.GetPrefabById(Convert.ToInt32(cmd.Split(' ')[1]));
                        break;
                    case "kill":
                        Kill(selectedPlayer);
                        break;
                    case "neighbor":
                        GameController.instance.players.neighborPlayer.net.SendReliable(SimpleHoloNetObjectMessage.Create<ExplorerTransformToNeighborMessage>(), DestinationGroup.All);
                        break;
                    case "windows":
                        foreach (Window win in GameObject.FindObjectsOfType<Window>())
                        {
                            win.net.SendReliable(WindowCrashMessage.Create(default, default), DestinationGroup.All);
                        }
                        break;
                    case "doors":
                        foreach (DoorInteractable door in GameObject.FindObjectsOfType<DoorInteractable>())
                        {
                            door.net.SendReliable(DoubleSidedDoorOpenMessage.Create(100f), DestinationGroup.All);
                            door.Close();
                        }
                        break;
                    case "lights":
                        foreach (LightInteractable light in GameObject.FindObjectsOfType<LightInteractable>())
                        {
                            light.net.SendReliable(SimpleHoloNetObjectMessage.Create<LightInteractableToggleLightMessage>(), DestinationGroup.All);
                        }
                        break;
                    case "items":
                        foreach (InventoryItem item in items)
                        {
                            HoloNet.SendReliable(NetObjectDestroyMessage.Create(item.net.oid), DestinationGroup.All);
                        }
                        break;
                    case "fakedeath":
                        FakeDeath();
                        break;
                    case "blind":
                        var pos = selectedPlayer.currentActor.transform.position;
                        Tp(selectedPlayer, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
                        break;
                    case "drop":
                        GetPlayerInventory(selectedPlayer).DropAll(InventoryItemType.STANDART);
                        break;
                    case "prefab":
                        System.Console.WriteLine(HoloNetAppModule.instance.objectsManager.GetPrefabById(Convert.ToInt32(cmd.Split(' ')[1])).name);
                        break;
                    case "freecam":
                        FreeCam();
                        break;
                    case "head":
                        var ld = localLobbyPlayer.explorerClassLoadout;
                        ld.customizationSetup.SetItem(head);
                        localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(localLobbyPlayer.playerInfo, ld, localLobbyPlayer.neighborClassLoadout), DestinationGroup.All);
                        break;
                    case "body":
                        var ld3 = localLobbyPlayer.explorerClassLoadout;
                        ld3.customizationSetup.SetItem(body);
                        localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(localLobbyPlayer.playerInfo, ld3, localLobbyPlayer.neighborClassLoadout), DestinationGroup.All);
                        break;
                    case "neighborlby":
                        localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(localLobbyPlayer.playerInfo, localLobbyPlayer.neighborClassLoadout, localLobbyPlayer.neighborClassLoadout), DestinationGroup.All);
                        break;
                    case "savehead":
                        head = localLobbyPlayer.explorerClassLoadout.customizationSetup.GetSlot(CustomizationSlot.HEAD);
                        break;
                    case "savebody":
                        body = localLobbyPlayer.explorerClassLoadout.customizationSetup.GetSlot(CustomizationSlot.TORSO);
                        break;
                        case "ids":
                        foreach (LobbyPlayer lbpl in allLobbyPlayers) System.Console.WriteLine($"{lbpl.displayName} - {lbpl.playerInfo.playerID}");
                        break;
                        case "uploadmap":
                            SteamWorkshopHelper.currentAsset = MapEditorController.instance.currentMapAsset;
                            SteamWorkshopHelper.UploadItem();
                            break;
                        case "lobbies":
                        GetServerList();
                        break;
                    case "copy":
                        var lo = FindLobbyplayerByName(cmd.Remove(0, 5));
                        var al = localLobbyPlayer.playerInfo;
                        al.displayName = lo.displayName;
                        localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(al, lo.explorerClassLoadout, lo.neighborClassLoadout), DestinationGroup.All);
                        break;
                    case "rename":
                        if (!animated)
                        {
                            var dff = localLobbyPlayer.playerInfo;
                            dff.displayName = cmd.Remove(0, 7);
                            localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(dff, localLobbyPlayer.explorerClassLoadout, localLobbyPlayer.neighborClassLoadout), DestinationGroup.All);
                        }
                        else
                        {
                            System.Console.WriteLine("Im here");
                            names = cmd.Remove(0, 7).Split('\n').ToList();
                            atName = 0;
                            System.Console.WriteLine("Got it\n" + cmd.Remove(0, 7));
                        }
                        break;
                    case "prefabs":
                        var mangr = HoloNetAppModule.instance.objectsManager;
                        string obj = string.Empty;
                        for (int a = 0; a < mangr._netPrefabs.Count; a++)
                        {
                            obj += $"{a.ToString()} - {mangr.GetPrefabById(a).name}\n";
                        }
                        using (System.IO.StreamWriter st = System.IO.File.CreateText(@"C:\Users\Serwis\Desktop\Prefabs.txt"))
                        {
                            st.Write(obj);
                            st.Close();
                        }
                        break;
                    case "spawn":
                        string name = cmd.Replace($"{cmd.Split(' ')[0]} {cmd.Split(' ')[1]} ", "");
                        var cam = GameCamera.instance.cam;
                        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
                        RaycastHit hit;
                        Physics.Raycast(ray, out hit);
                        
                        System.Console.WriteLine(hit.point.ToString());
                        SpawnItems(name, Convert.ToInt32(cmd.Split(' ')[1]), (hit.distance < 4f ? hit.point : cam.transform.position + cam.transform.forward * 4f), default);
                        break;
                    case "fuckinghead":
                        var lde = localLobbyPlayer.explorerClassLoadout;
                        var hed = lde.customizationSetup.GetSlot(CustomizationSlot.HEAD);
                        hed.actorClass = null;
                        hed.actorClassId = "secks";
                        localLobbyPlayer.net.SendReliable(LobbyPlayerSyncInfoMessage.Create(localLobbyPlayer.playerInfo, lde, localLobbyPlayer.neighborClassLoadout), DestinationGroup.All);
                        break;
                    case "end":
                        EndLevelType end;
                        switch (cmd.Split(' ')[1])
                        {
                            case "0":
                                end = EndLevelType.ALL_CHILDREN_DEAD;
                                break;
                            case "1":
                                end = EndLevelType.BASEMENT_ENTERED;
                                break;
                            case "2":
                                end = EndLevelType.TIME_IS_UP;
                                break;
                            case "3":
                                end = EndLevelType.QUEST_COMPLETED;
                                break;
                            default:
                                end = EndLevelType.ALL_CHILDREN_DEAD;
                                break;
                        }
                        HoloNet.SendReliable(GameEndedMessage.Create(end), DestinationGroup.All);
                        break;
                    case "buff":
                        BuffId buff;
                        switch (cmd.Split(' ')[1])
                        {
                            case "0":
                                buff = BuffId.INVINCIBLE;
                                break;
                            case "1":
                                buff = BuffId.KNOCK;
                                break;
                            case "2":
                                buff = BuffId.CONTROL_GATES_BUFF; 
                                break;
                            case "3":
                                buff = BuffId.BLIND; 
                                break;
                            case "4":
                                buff = BuffId.DISABLE_ALL_EXCEPT_CAMERA;
                                break;
                            case "5":
                                buff = BuffId.SECRET_DOOR_CONTROL;
                                break;
                            case "6":
                                buff = BuffId.TOMATO;
                                break;
                            case "7":
                                buff = BuffId.SPEEDUP;
                                break;
                            case "8":
                                buff = BuffId.THROW_FORCE_BOOST;
                                break;
                            case "9":
                                buff = BuffId.SPEEDUP;
                                Buff(BuffId.THROW_FORCE_BOOST, selectedPlayer);
                                break;
                            default:
                                buff = BuffId.KNOCK;
                                break;
                        }
                        Buff(buff, selectedPlayer);
                        break;
                    case "rembuff":
                        BuffId rembuff;
                        switch (cmd.Split(' ')[1])
                        {
                            case "0":
                                rembuff = BuffId.INVINCIBLE;
                                break;
                            case "1":
                                rembuff = BuffId.KNOCK;
                                break;
                            case "2":
                                rembuff = BuffId.CONTROL_GATES_BUFF;
                                break;
                            case "3":
                                rembuff = BuffId.BLIND;
                                break;
                            case "4":
                                rembuff = BuffId.DISABLE_ALL_EXCEPT_CAMERA;
                                break;
                            case "5":
                                rembuff = BuffId.SECRET_DOOR_CONTROL;
                                break;
                            case "6":
                                rembuff = BuffId.TOMATO;
                                break;
                            case "7":
                                rembuff = BuffId.SPEEDUP;
                                break;
                            case "8":
                                rembuff = BuffId.THROW_FORCE_BOOST;
                                break;
                            case "9":
                                rembuff = BuffId.SPEEDUP;
                                RemBuff(BuffId.THROW_FORCE_BOOST, selectedPlayer);
                                break;
                            default:
                                rembuff = BuffId.KNOCK;
                                break;
                        }
                        RemBuff(rembuff, selectedPlayer);
                        break;
                    case "tp":
                        if (cmd.ToLower().Split(' ')[1] == "all")
                        {
                            foreach (Player plr in allPlayers)
                            {
                                Tp(plr, FindPlayerByName(cmd.Split(' ')[2]).currentActor.transform.position);
                            }
                        }
                        else Tp(FindPlayerByName(tpPlr), FindPlayerByName(tpToPlr).currentActor.transform.position);
                        break;
                    case "token":
                        System.Console.WriteLine(AppController.instance.platform.GetAuthToken());
                        break;
                    case "box":
                        localPlayer.currentActor.Deactivate();
                            freeCam = new GameObject();
                            freeCam.AddComponent<Camera>();
                            GameCamera.instance.cam.enabled = false;
                            var mana = HoloNetAppModule.instance.objectsManager;
                            var pre = mana.GetPrefabById(67);
                            box = mana.Spawn(pre, default, default);
                            box.transform.parent = freeCam.transform;
                            box.transform.localPosition = Vector3.zero;
                            isBox = true;
                            isFreeCam = true;
                            break;
                        default:
                        System.Console.WriteLine("This command doesn't exist");
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Something went wrong\n" + ex.Message);
            }
            cmd = string.Empty;
        }

        private void GetServerList()
        {
            serverList = string.Empty;
            foreach (ConnectButtonUi btn in GameObject.FindObjectsOfType<ConnectButtonUi>())
            {
                var room = btn._networkRoomData;
                serverList += $"Name: \"{room.name}\" - Password: \"{room.password}\"\n";
            }
            System.Console.WriteLine(serverList);
        }

        public void HandleGameplayHacks()
        {
            var cam = GameCamera.instance.cam;
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (Input.GetKeyDown(KeyCode.K))
            {
                foreach (Component com in hit.collider.gameObject.GetComponents<Component>())
                {
                    System.Console.WriteLine(com.ToString());
                }
                foreach (Component com in hit.collider.gameObject.GetComponentsInChildren<Component>())
                {
                    System.Console.WriteLine("(Child) " + com.ToString());
                }
            }
            lookingAt = hit.collider.gameObject.name; // + (hit.collider.gameObject.GetComponentInChildren<HoloNetObject>() != null ? " (HoloNet)" : "");
            if (Input.GetMouseButton(0))
            {
                var mana = HoloNetAppModule.instance.objectsManager;
                var pre = mana.GetPrefabById(52);
                var appl = mana.Spawn(pre, hit.point, default);
                appl.gameObject.GetComponent<Rigidbody>().Sleep();
            }
            //System.Console.WriteLine($"x: {localPlayer.currentActor.transform.position.x}, y: {localPlayer.currentActor.transform.position.y}, z: {localPlayer.currentActor.transform.position.z}");
            if (Input.GetKeyDown(KeyCode.L)) cmd = "spawn 1 apple";
            if (!checkedNeighbor)
            {
                playerNames.Clear();
                foreach (LobbyPlayer lb in allLobbyPlayers) playerNames.Add(lb.displayName);
                selectedPlayer = localPlayer;
                selectedName = GetLobbyplayer(localPlayer).displayName;
                neighborName = GetLobbyplayer(GameController.instance.players.neighborPlayer).displayName;
                checkedNeighbor = true;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                noclip = !noclip;
                localPlayer.currentActor.GetComponent<Collider>().enabled = true;
            }
            if (Input.GetKey(KeyCode.X))
            {
                Buff(BuffId.SPEEDUP, localPlayer);
                Buff(BuffId.THROW_FORCE_BOOST, localPlayer);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                localPlayer.net.SendReliable(NeighborLevelUpMessage.Create(999), DestinationGroup.All);
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                var a = UnityEngine.Object.FindObjectOfType<PlayersListUi>();
                if (!a.showNeighborPlayer || !a.showCatchedPlayers)
                {
                    a.showNeighborPlayer = true;
                    a.showCatchedPlayers = true;
                }
            }
            
            //if (Input.GetMouseButtonDown(0))
            //{
            //    foreach (var itm in items)
            //    {
            //        if (itm.name.ToLower().Contains("rifle"))
            //        {
            //            var rifle = itm.GetComponent<RifleInventoryItem>();
            //            if (rifle.isCarried && rifle.player == localPlayer)
            //            {
            //                if (!rifle._charged) rifle._charged = true;
            //            }
            //        }
            //    }
            //}
            //if (Input.GetKeyDown(KeyCode.KeypadEnter))
            //{
            //    foreach (Player player in this.allPlayers)
            //    {
            //        player.net.SendReliable(SimpleHoloNetObjectMessage.Create<PlayerEnterBasementMessage>(), DestinationGroup.All);
            //    }
            //    HoloNet.SendReliable(GameEndedMessage.Create(EndLevelType.BASEMENT_ENTERED), DestinationGroup.All);
            //}
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                foreach (Player player in this.allPlayers)
                {
                    Kill(player);
                }
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                HoloNet.SendReliable(GameEndedMessage.Create(EndLevelType.QUEST_COMPLETED), DestinationGroup.All);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                localPlayer.net.SendReliable(HandleDeathMessage.Create(PlayerDeathReason.NEIGHBOR_KILL), DestinationGroup.All);
            }

            if (noclip)
            {
                localPlayer.currentActor.GetComponent<Collider>().enabled = false;
                localPlayer.currentActor.transform.position += Input.GetAxis("Vertical") * GameCamera.instance.cam.transform.forward * Time.deltaTime * 15 + Input.GetAxis("Horizontal") * localPlayer.currentActor.transform.right * Time.deltaTime * 15;
                if (Input.GetKey(KeyCode.Space)) localPlayer.currentActor.transform.position += localPlayer.currentActor.transform.up * Time.deltaTime * 15;
                else if (Input.GetKey(KeyCode.LeftControl)) localPlayer.currentActor.transform.position -= localPlayer.currentActor.transform.up * Time.deltaTime * 15;
            }
            if (isBox)
            {
                box.transform.position = freeCam.transform.position;
            }
        }

        public void HandleLobbyHacks()
        {
            //lastJoin++;
            //if (lastJoin < 100) return;
            //if (!localLobbyPlayer) return;
            //HoloNet.SendReliable(NetDisconnectMessage.Create("Fuck you tiny and holo", "Fuck you tiny and holo", HoloNet.currentRoom.id, true), DestinationGroup.Others);
            //HoloNet.ChangeRoomProperties(new RoomSettings
            //{
            //    name = "Attempt to kill the game",
            //    isVisible = HoloNet.currentRoom.isVisible,
            //    isOpen = HoloNet.currentRoom.isOpen,
            //    locale = HoloNet.currentRoom.locale,
            //    maxPlayers = (byte)HoloNet.currentRoom.maxPlayers,
            //    password = "thicc",
            //    randomPlayers = HoloNet.currentRoom.randomPlayers,
            //    roomType = HoloNet.currentRoom.roomType
            //});
            //LobbyController.instance.LeaveLobby();

            if (antiKick)
            {
                var ad = GameObject.FindObjectOfType<GamesListPanelUi>().customGameScreen._targetNetworkRoomData;
                if (ad != null) LobbyController.instance.JoinCustomGame(ad);
            }
            foreach (var lb in allLobbyPlayers)
            {
                if (lb.name.ToLower().Contains("dylan")) HoloNet.SendReliable(PlayerDisconnectedMessage.Create(lb.net.oid), DestinationGroup.All);
            }
            if (animated && names.Count > 0 && localLobbyPlayer.state != LobbyPlayerState.LOADING_LEVEL)
            {
                minisecs += Time.deltaTime * 1000;
                if (minisecs >= 100)
                {
                    AnimateName();
                    minisecs = 0;
                }
            }
            
            checkedNeighbor = false;
            lobbyName = HoloNet.currentRoom.name;
            lobbyPass = HoloNet.currentRoom.password;
            
            //System.Console.WriteLine($"x: {freeCam.transform.position.x}, y: {freeCam.transform.position.y}, z: {freeCam.transform.position.z}");
        }

        private void HandleMenuHacks()
        {
            //if (GameObject.FindObjectOfType<DialogWindow>()) GameObject.FindObjectOfType<DialogWindow>().Hide();
            //LobbyController.instance.JoinCustomGame(GameObject.FindObjectOfType<ConnectButtonUi>()._networkRoomData);
        }

        private string DecryptText4(string rawText)
        {
            try
            {
                char[] thingChars = rawText.Remove(rawText.Length - 1).ToCharArray();
                Array.Reverse(thingChars);
                string text = new string(thingChars);
                string convertor = string.Empty;
                int times = rawText[rawText.Length - 1] - '0';

                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                for (var i = 0; i < text.Length; i += 3)
                    list.Add(text.Substring(i, Math.Min(3, text.Length - i)));
                foreach (string str in list)
                {
                    convertor += ((char)(int.Parse(str) / times)).ToString();
                }
                if (!convertor.Contains("ab=ba")) return "error";
                var a = convertor.Split(new[] { "ab=ba" }, StringSplitOptions.None);
                string retardedText = string.Empty;
                int l, n;
                char ch;
                l = a[1].Length;
                var arr1 = a[1].ToCharArray();
                for (n = 0; n < l; n++)
                {
                    ch = arr1[n];
                    if (Char.IsLower(ch))
                        retardedText += Char.ToUpper(ch);
                    else
                        retardedText += Char.ToLower(ch);
                }
                if (retardedText == a[0]) return a[0];
                else return "error";
            }
            catch
            {
                return "error";
            }
        }

        private HoloNetObject box;
        private bool isBox = false;
        private bool checkedNeighbor = false;
        private bool noclip = false;
        private bool plrChecked = false;
        private GameObject freeCam;
        private bool isFreeCam = false;
        private CustomizationItemInfo head;
        private CustomizationItemInfo body;
        private GameObject prefab;
        private string oldName;
        private int check = 0;
        private bool disallowed = false;
        //private DiscordSocketClient client;
        public static string lookingAt;
        public static bool animated = false;
        public static bool menu = false;
        public static bool antiKick = false;
        public static string serverList = "";
        public static string neighborName = "";
        public static string status = "";
        public static string lobbyName = "";
        public static string lobbyPass = "";
        public static string tpPlr = "";
        public static string tpToPlr = "";
        private bool discordReady = false;
        private Player selectedPlayer;
        private LobbyPlayer selectedLobbyPlayer;
        public static string selectedName = "";
        public static string selectedLobbyName = "";
        public static string cmd = string.Empty;
        public static int mode = 0;
        private int atName = 0;
        private float minisecs = 0;
        private System.Collections.Generic.List<string> whitelist = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<string> bans = new System.Collections.Generic.List<string>();
        public static System.Collections.Generic.List<string> playerNames = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<string> names = new System.Collections.Generic.List<string>();
        private Player localPlayer
        {
            get
            {
                return GameController.instance.players.localPlayer;
            }
        }
        private LobbyPlayer localLobbyPlayer
        {
            get
            {
                foreach (var a in allLobbyPlayers)
                {
                    if (a.isLocal) return a;
                }
                return null;
            }
        }
        public List<Player> allPlayers
        {
            get
            {
                return GameController.instance.players.allPlayers;
            }
        }
        public Vector3 randomPos
        {
            get
            {
                int x = UnityEngine.Random.Range(-17, 30);
                int z = UnityEngine.Random.Range(-31, 24);
                return new Vector3(x, 35, z);
            }
        }
        public List<LobbyPlayer> allLobbyPlayers
        {
            get
            {
                return LobbyController.instance.players.lobbyPlayers;
            }
        }
        private IEnumerable<HoloNetObject> objects
        {
            get
            {
                return HoloNetAppModule.instance.objectsManager.FindAllSceneObjects();
            }
        }
        private InventoryItem[] items
        {
            get
            {
                return UnityEngine.Object.FindObjectsOfType<InventoryItem>();
            }
        }

        

        /*public override void OnGUI()
        {
            if (AppController.instance.modes.currentGameMode == GameModes.Base.GameModeId.LOBBY)
            {
                GUI.backgroundColor = Color.white;
                GUI.Box(new Rect(800, 10, 790, 80), "Players");
                var lps = Resources.FindObjectsOfTypeAll<LobbyPlayer>();
                if (GUI.Button(new Rect(810, 20f, 120, 60), lps[0].displayName)) KickPlayer(lps[0]);
                if (GUI.Button(new Rect(940, 20f, 120, 60), lps[1].displayName)) KickPlayer(lps[1]);
                if (GUI.Button(new Rect(1070, 20f, 120, 60), lps[2].displayName)) KickPlayer(lps[2]);
                if (GUI.Button(new Rect(1200, 20f, 120, 60), lps[3].displayName)) KickPlayer(lps[3]);
                if (GUI.Button(new Rect(1330, 20f, 120, 60), lps[4].displayName)) KickPlayer(lps[4]);
                if (GUI.Button(new Rect(1460, 20f, 120, 60), lps[5].displayName)) KickPlayer(lps[5]);
            }
            if (this.menu && AppController.instance.modes.currentGameMode == GameModes.Base.GameModeId.GAMEPLAY)
            {
                try
                {
                    if (GUI.Button(new Rect(25f, 530f, 100f, 80f), GetLobbyplayer(this.allPlayers[0]).displayName))
                    {
                        this.chosenPlayer = this.allPlayers[0];
                    }
                    if (GUI.Button(new Rect(150f, 530f, 100f, 80f), GetLobbyplayer(this.allPlayers[1]).displayName))
                    {
                        this.chosenPlayer = this.allPlayers[1];
                    }
                    if (GUI.Button(new Rect(275f, 530f, 100f, 80f), GetLobbyplayer(this.allPlayers[2]).displayName))
                    {
                        this.chosenPlayer = this.allPlayers[2];
                    }
                    if (GUI.Button(new Rect(25f, 635f, 100f, 80f), GetLobbyplayer(this.allPlayers[3]).displayName))
                    {
                        this.chosenPlayer = this.allPlayers[3];
                    }
                    if (GUI.Button(new Rect(150f, 635f, 100f, 80f), GetLobbyplayer(this.allPlayers[4]).displayName))
                    {
                        this.chosenPlayer = this.allPlayers[4];
                    }
                    if (GUI.Button(new Rect(275f, 635f, 100f, 80f), GetLobbyplayer(this.allPlayers[5]).displayName))
                    {
                        this.chosenPlayer = this.allPlayers[5];
                    }
                }
                catch
                {
                }
                GUI.Box(new Rect(0f, 0f, 400f, 735f), "Menu");
                //if (GUI.Button(new Rect(5f, 25f, 90f, 50f), "Kill player"))
                //{
                //    this.chosenPlayer.currentActor.healthHandler.Die(ActorDeathReason.NEIGHBOR_KILL);
                //}
                if (GUI.Button(new Rect(100f, 25f, 90f, 50f), "Knock player"))
                {
                    this.chosenPlayer.currentActor.player.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.KNOCK, null);
                }
                if (GUI.Button(new Rect(195f, 25f, 90f, 50f), "Catch player"))
                {
                    this.chosenPlayer.currentActor.player.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.CATCH, null);
                }
                if (GUI.Button(new Rect(290f, 25f, 90f, 50f), "Freeze player"))
                {
                    this.chosenPlayer.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.DISABLE_ALL_EXCEPT_CAMERA, null);
                }
                if (GUI.Button(new Rect(5f, 80f, 90f, 50f), "Unfreeze player"))
                {
                    this.chosenPlayer.GetComponent<PlayerBuffs>().VanishBuff(BuffId.DISABLE_ALL_EXCEPT_CAMERA);
                }
                if (GUI.Button(new Rect(100f, 80f, 90f, 50f), "Help player"))
                {
                    this.chosenPlayer.currentActor.player.GetComponent<PlayerBuffs>().VanishBuff(BuffId.KNOCK);
                    this.chosenPlayer.currentActor.player.GetComponent<PlayerBuffs>().VanishBuff(BuffId.KNOCK_DEAD);
                    this.chosenPlayer.currentActor.player.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.SPEEDUP, null);
                    this.chosenPlayer.currentActor.player.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.THROW_FORCE_BOOST, null);
                    this.chosenPlayer.GetComponent<PlayerHealth>().HealBy(200);
                }
                if (GUI.Button(new Rect(290f, 80f, 90f, 50f), "Kids win"))
                {
                    foreach (Player player in this.allPlayers)
                    {
                        player.net.SendReliable(SimpleHoloNetObjectMessage.Create<PlayerEnterBasementMessage>(), DestinationGroup.All);
                    }
                    HoloNet.SendReliable(GameEndedMessage.Create(EndLevelType.BASEMENT_ENTERED), DestinationGroup.All);
                }
                if (GUI.Button(new Rect(5f, 135f, 90f, 50f), "Neighbor win"))
                {
                    HoloNet.SendReliable(GameEndedMessage.Create(EndLevelType.ALL_CHILDREN_DEAD), DestinationGroup.All);
                }
                if (GUI.Button(new Rect(195f, 135f, 90f, 50f), "Kick player"))
                {
                    this.chosenPlayer.GetComponent<LobbyPlayer>().KickPlayer();
                }
                //if (GUI.Button(new Rect(100f, 135f, 90f, 50f), "Respawn player"))
                //{
                //    this.chosenPlayer.currentActor.GetComponent<PlayerHealth>().R
                //}
                if (GUI.Button(new Rect(290f, 135f, 90f, 50f), "Tp keys"))
                {
                    foreach (InventoryItem inventoryItem in this.items)
                    {
                        if (inventoryItem.rarity == ItemRarity.LEGENDARY)
                        {
                            this.localPlayer.net.GetComponent<PlayerInventory>().DropAll(InventoryItemType.STANDART);
                            this.localPlayer.net.GetComponent<PlayerInventory>().TakeImmidiate(inventoryItem, null);
                        }
                    }
                }
                if (GUI.Button(new Rect(5f, 190f, 90f, 50f), "Tp keycards"))
                {
                    this.tpItems("card");
                }
                if (GUI.Button(new Rect(100f, 190f, 90f, 50f), "Tp\nconsumables"))
                {
                    this.tpItems("milk");
                    this.tpItems("chocolate");
                }
                if (GUI.Button(new Rect(195f, 190f, 90f, 50f), "Tp rifle(s)"))
                {
                    this.tpItems("rifle");
                }
                if (GUI.Button(new Rect(290f, 190f, 90f, 50f), "Tp all"))
                {
                    foreach (InventoryItem inventoryItem2 in this.items)
                    {
                        if (!inventoryItem2.gameObject.name.ToLower().Contains("box"))
                        {
                            this.localPlayer.GetComponent<PlayerInventory>().DropAll(InventoryItemType.STANDART);
                            this.localPlayer.GetComponent<PlayerInventory>().TakeImmidiate(inventoryItem2, null);
                        }
                    }
                }
                if (GUI.Button(new Rect(5f, 245f, 90f, 50f), "Tp custom"))
                {
                    this.tpItem = true;
                    this.menu = false;
                    this.localPlayer.LockInput();
                    this.localPlayer.BlockActorSwitch();
                }
                if (GUI.Button(new Rect(100f, 245f, 90f, 50f), "Transform to\nneighbor"))
                {
                    this.chosenPlayer.net.SendReliable(SimpleHoloNetObjectMessage.Create<ExplorerTransformToNeighborMessage>(), DestinationGroup.All);
                }
                if (GUI.Button(new Rect(195f, 245f, 90f, 50f), "Transform to\nexplorer"))
                {
                    this.chosenPlayer.net.SendReliable(SimpleHoloNetObjectMessage.Create<NeighborTransformToExplorerMessage>(), DestinationGroup.All);
                }
                if (GUI.Button(new Rect(290f, 245f, 90f, 50f), "Crash windows"))
                {
                    Window[] array = this.window;
                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j].net.SendReliable(WindowCrashMessage.Create(default(Vector3), default(Vector3)), DestinationGroup.All);
                    }
                }
                if (GUI.Button(new Rect(5f, 300f, 90f, 50f), "Open doors"))
                {
                    DoorInteractable[] array2 = this.doors;
                    for (int k = 0; k < array2.Length; k++)
                    {
                        array2[k].net.SendReliable(DoubleSidedDoorOpenMessage.Create(100f), DestinationGroup.All);
                        array2[k].Close();
                    }
                }
                if (GUI.Button(new Rect(100f, 300f, 90f, 50f), "Flicker lights"))
                {
                    LightInteractable[] array3 = this.light;
                    for (int l = 0; l < array3.Length; l++)
                    {
                        array3[l].net.SendReliable(SimpleHoloNetObjectMessage.Create<LightInteractableToggleLightMessage>(), DestinationGroup.All);
                    }
                }
                if (GUI.Button(new Rect(195f, 300f, 90f, 50f), "Ghost mode"))
                {
                    HoloNet.SendReliable(NetPlayerDisconnectedMessage.Create(GetLobbyplayer(localPlayer).owner), DestinationGroup.Others);
                }
                if (GUI.Button(new Rect(290f, 300f, 90f, 50f), "?"))
                {
                    HoloNet.SendReliable(GameEndedMessage.Create(EndLevelType.QUEST_COMPLETED), DestinationGroup.All);
                }
                if (GUI.Button(new Rect(5f, 355f, 90f, 50f), "Toggle\nlow gravity"))
                {
                    this.speed = !this.speed;
                    if (this.speed)
                    {
                        this.localPlayer.currentActor.GetComponent<ActorMovement>().config.gravityMultilayer = 0.5f;
                    }
                    else
                    {
                        this.localPlayer.currentActor.GetComponent<ActorMovement>().config.gravityMultilayer = 1.3f;
                    }
                }
                if (GUI.Button(new Rect(100f, 355f, 90f, 50f), "Delete timer"))
                {
                    HoloNet.SendReliable(LevelTimerStartedMessage.Create(0f, EndLevelType.TIME_IS_UP), DestinationGroup.All);
                }
                if (GUI.Button(new Rect(195f, 355f, 90f, 50f), "Print a photo\n(promote hacks)"))
                {
                    UnityEngine.Object.FindObjectOfType<QuestPrinter>().net.SendReliable(PrintPhotoValidateInMaster.Create("<color=red>Get hacks today!</color>\n<color=yellow>Piki#1234</color>"), DestinationGroup.All);
                    this.tpItems("photo");
                }
                if (GUI.Button(new Rect(290f, 355f, 90f, 50f), "Give godmode"))
                {
                    this.chosenPlayer.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.INVINCIBLE, null);
                }
                if (GUI.Button(new Rect(5f, 410f, 90f, 50f), "Take godmode"))
                {
                    this.chosenPlayer.GetComponent<PlayerBuffs>().VanishBuff(BuffId.BLIND);
                }
                if (GUI.Button(new Rect(100f, 410f, 90f, 50f), "Give blind\neffect"))
                {
                    this.chosenPlayer.GetComponent<PlayerBuffs>().ApplyBuff(BuffId.BLIND, null);
                }
                if (GUI.Button(new Rect(195f, 410f, 90f, 50f), "Delete all\nitems"))
                {
                    InventoryItem[] items2 = this.items;
                    for (int m = 0; m < items2.Length; m++)
                    {
                        HoloNet.SendReliable(NetObjectDestroyMessage.Create(items2[m].net.oid), DestinationGroup.All);
                    }
                }
                //if (GUI.Button(new Rect(290f, 410f, 90f, 50f), "Destroy ground"))
                //{
                //    for (int a = 0; a < )
                //    {
                //        if (objects[a].name.ToLower().Contains("terrain"))
                //        {
                //            HoloNet.SendReliable(NetObjectDestroyMessage.Create(objects[a].oid), DestinationGroup.All);
                //        }
                //    }
                //}
                if (GUI.Button(new Rect(5f, 465f, 90f, 50f), "Spawn"))
                {
                    HoloNet.SpawnSceneObjectLocal(Config<CrowConfig>.instance.crowPrefab).transform.position = this.localPlayer.currentActor.transform.position;
                    HoloNet.SpawnNetObject(Config<CrowConfig>.instance.crowPrefab, this.localPlayer.currentActor.transform.position, default(Quaternion));
                }
            }
            GUIStyle guistyle = new GUIStyle();
            guistyle.normal.textColor = Color.cyan;
            guistyle.fontSize = 20;
            try
            {
                GUI.Label(new Rect(400f, 10f, 200f, 20f), "Selected player: " + GetLobbyplayer(chosenPlayer).displayName, guistyle);
            }
            catch
            {
            }
            /*try
            {
                foreach (Player player2 in this.allPlayers)
                {
                    if (!player2.isLocal)
                    {
                        Vector3 vector = GameController.instance.cameras.currentCamera.cam.WorldToScreenPoint(player2.currentActor.centerTransform.position);
                        if (vector.z > 0f)
                        {
                            GUIStyle guistyle2 = new GUIStyle();
                            guistyle2.alignment = TextAnchor.UpperCenter;
                            guistyle2.normal.textColor = (player2.isNeighbor ? Color.red : Color.blue);
                            guistyle2.fontSize = 15;
                            vector.y = (float)Screen.height - (vector.y + 1f);
                            //float num3 = player2.gameObject.GetComponent<PlayerHealth>().healthPercentage * 100f;
                            GUI.Label(new Rect(vector.x - 45f, vector.y - 40f, 100f, 50f), GetLobbyplayer(player2).displayName + "\n" + num3.ToString() + " hp", guistyle2);
                        }
                    }
                }
            }
            catch
            {
            }
        }*/
    }
}
