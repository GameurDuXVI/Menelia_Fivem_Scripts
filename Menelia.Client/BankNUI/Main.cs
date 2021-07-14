using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MeneliaAPI.Client;
using MeneliaAPI.Entities;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace BankNUI
{
    public class Main : BaseScript
    {
        bool display = false;
        public Main()
        {
            RegisterCommand("bank", new Action<int, List<object>, string>((source, args, raw) =>
            {
                setDisplay(!display);
            }), false);

            Tick += OnTick;

            RegisterNuiCallbackType("main");
            EventHandlers["__cfx_nui:main"] += new Action(main);


            RegisterNuiCallbackType("withdrawal");
            EventHandlers["__cfx_nui:withdrawal"] += new Action<IDictionary<string, object>, CallbackDelegate>(async (data, cb) =>
            {
                if (data.TryGetValue("amount", out var obj))
                {
                    int amount = Convert.ToInt32(obj);
                    PlayerInfo pi = PlayerInfo.FromJson(await ClientUtils.GetPlayerInfo());
                    if(pi.Banking.Money < amount)
                    {
                        int diff = amount - pi.Banking.Money;
                        pi.Banking.Money = 0;
                        pi.Cash += diff;
                        pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, pi.Name, "Retrait", -diff));
                    }
                    else
                    {
                        pi.Banking.Money -= amount;
                        pi.Cash += amount;
                        pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, pi.Name, "Retrait", -amount));
                    }
                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.UpdatePlayerInfo(pi) + "}");
                }

                cb(new {
                    error = "Amount not found !"
                });
            });

            RegisterNuiCallbackType("deposit");
            EventHandlers["__cfx_nui:deposit"] += new Action<IDictionary<string, object>, CallbackDelegate>(async (data, cb) =>
            {
                if (data.TryGetValue("amount", out var obj))
                {
                    int amount = Convert.ToInt32(obj);
                    PlayerInfo pi = PlayerInfo.FromJson(await ClientUtils.GetPlayerInfo());
                    if (pi.Cash < amount)
                    {
                        int diff = amount - pi.Cash;
                        pi.Cash -= amount;
                        pi.Banking.Money += diff;
                        pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, pi.Name, "Dépôt", diff));
                    }
                    else
                    {
                        pi.Cash -= amount;
                        pi.Banking.Money += amount;
                        pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, pi.Name, "Dépôt", amount));
                    }
                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.UpdatePlayerInfo(pi) + "}");
                }

                cb(new
                {
                    error = "Amount not found !"
                });
            });

            RegisterNuiCallbackType("transfer");
            EventHandlers["__cfx_nui:transfer"] += new Action<IDictionary<string, object>, CallbackDelegate>(async (data, cb) =>
            {
                if (data.TryGetValue("amount", out var ObjAmount) && data.TryGetValue("reason", out var ObjReason) && data.TryGetValue("receiver", out var ObjReceiver))
                {
                    String Receiver = (String)ObjReceiver;
                    int Amount = Convert.ToInt32(ObjAmount);
                    String Reason = (String)ObjReason;

                    List<PlayerInfo> PlayerInfos = PlayerInfo.ListFromJson(await ClientUtils.GetPlayerInfos());
                    PlayerInfo Pi = PlayerInfo.FromJson(await ClientUtils.GetPlayerInfo());

                    foreach (PlayerInfo PiR in PlayerInfos)
                    {
                        if (PiR.Name.ToLower().Equals(Receiver.ToLower()))
                        {
                            if (Pi.Banking.Money < Amount)
                            {
                                int diff = Amount - Pi.Banking.Money;
                                Pi.Banking.Money -= diff;
                                PiR.Banking.Money += diff;
                                Pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, PiR.Name, "Transaction", -diff));
                                PiR.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, Pi.Name, "Transaction", diff));
                            }
                            else
                            {
                                Pi.Banking.Money -= Amount;
                                PiR.Banking.Money += Amount;
                                Pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, PiR.Name, "Transaction", -Amount));
                                PiR.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, Pi.Name, "Transaction", Amount));
                            }
                            await ClientUtils.UpdatePlayerInfo(PiR);
                            await ClientUtils.UpdatePlayerInfo(Pi);
                            break;
                        }
                    }

                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.GetPlayerInfo() + "}");
                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"page\", \"data\": \"bank-main-page\"}");
                }

                cb(new
                {
                    error = "Amount not found !"
                });
            });





            RegisterNuiCallbackType("close");
            EventHandlers["__cfx_nui:close"] += new Action(close);
        }

        public void main()
        {
            ClientUtils.SendChatMessage("", "main", 255, 0, 0);
        }

        public void close()
        {
            setDisplay(false);
        }
        public async void setDisplay(bool display)
        {
            if (display)
            {
                SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.GetPlayerInfo() + "}");
                SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfos\", \"data\": " + await ClientUtils.GetPlayerInfos() + "}");
            }
            this.display = display;
            SetNuiFocus(display, display);
            SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"show\", \"data\": \"" + display + "\"}");
        }

        public async Task OnTick()
        {
            /**
                HUD = 0;
                HUD_WANTED_STARS = 1;
                HUD_WEAPON_ICON = 2;
                HUD_CASH = 3;
                HUD_MP_CASH = 4;
                HUD_MP_MESSAGE = 5;
                HUD_VEHICLE_NAME = 6;
                HUD_AREA_NAME = 7;
                HUD_VEHICLE_CLASS = 8;
                HUD_STREET_NAME = 9;
                HUD_HELP_TEXT = 10;
                HUD_FLOATING_HELP_TEXT_1 = 11;
                HUD_FLOATING_HELP_TEXT_2 = 12;
                HUD_CASH_CHANGE = 13;
                HUD_RETICLE = 14;
                HUD_SUBTITLE_TEXT = 15;
                HUD_RADIO_STATIONS = 16;
                HUD_SAVING_GAME = 17;
                HUD_GAME_STREAM = 18;
                HUD_WEAPON_WHEEL = 19;
                HUD_WEAPON_WHEEL_STATS = 20;
                MAX_HUD_COMPONENTS = 21;
                MAX_HUD_WEAPONS = 22;
                MAX_SCRIPTED_HUD_COMPONENTS = 141;
             */
            HideHudComponentThisFrame(3);
            HideHudComponentThisFrame(4);
            HideHudComponentThisFrame(13);
        }
    }
}
