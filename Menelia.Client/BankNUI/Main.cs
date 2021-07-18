using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using Menelia.Entities;
using static CitizenFX.Core.Native.API;

namespace Menelia.Client.BankNUI
{
    public class Bank : BaseScript
    {
        private bool _display;
        public Bank()
        {
            RegisterCommand("bank", new Action<int, List<object>, string>((source, args, raw) =>
            {
                setDisplay(!_display);
            }), false);

            Tick += onTick;

            RegisterNuiCallbackType("main");
            EventHandlers["__cfx_nui:main"] += new Action(main);


            RegisterNuiCallbackType("withdrawal");
            EventHandlers["__cfx_nui:withdrawal"] += new Action<IDictionary<string, object>, CallbackDelegate>(async (data, cb) =>
            {
                if (data.TryGetValue("amount", out var obj))
                {
                    var amount = Convert.ToInt32(obj);
                    var pi = PlayerInfo.fromJson(await ClientUtils.getPlayerInfo());
                    if(pi.Banking.Money < amount)
                    {
                        var diff = amount - pi.Banking.Money;
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
                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.updatePlayerInfo(pi) + "}");
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
                    var amount = Convert.ToInt32(obj);
                    var pi = PlayerInfo.fromJson(await ClientUtils.getPlayerInfo());
                    if (pi.Cash < amount)
                    {
                        var diff = amount - pi.Cash;
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
                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.updatePlayerInfo(pi) + "}");
                }

                cb(new
                {
                    error = "Amount not found !"
                });
            });

            RegisterNuiCallbackType("transfer");
            EventHandlers["__cfx_nui:transfer"] += new Action<IDictionary<string, object>, CallbackDelegate>(async (data, cb) =>
            {
                if (data.TryGetValue("amount", out var objAmount) && data.TryGetValue("reason", out var objReason) && data.TryGetValue("receiver", out var objReceiver))
                {
                    var receiver = (string)objReceiver;
                    var amount = Convert.ToInt32(objAmount);
                    var reason = (string)objReason;

                    var playerInfos = PlayerInfo.listFromJson(await ClientUtils.getPlayerInfos());
                    var pi = PlayerInfo.fromJson(await ClientUtils.getPlayerInfo());

                    foreach (var piR in playerInfos)
                    {
                        if (piR.Name.ToLower().Equals(receiver.ToLower()))
                        {
                            if (pi.Banking.Money < amount)
                            {
                                var diff = amount - pi.Banking.Money;
                                pi.Banking.Money -= diff;
                                piR.Banking.Money += diff;
                                pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, piR.Name, "Transaction", -diff));
                                piR.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, pi.Name, "Transaction", diff));
                            }
                            else
                            {
                                pi.Banking.Money -= amount;
                                piR.Banking.Money += amount;
                                pi.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, piR.Name, "Transaction", -amount));
                                piR.Banking.Transactions.Insert(0, new Transaction(DateTime.Now, pi.Name, "Transaction", amount));
                            }
                            await ClientUtils.updatePlayerInfo(piR);
                            await ClientUtils.updatePlayerInfo(pi);
                            break;
                        }
                    }

                    SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.getPlayerInfo() + "}");
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

        private void main()
        {
            ClientUtils.sendChatMessage("", "main", 255, 0, 0);
        }

        private void close()
        {
            setDisplay(false);
        }
        private async void setDisplay(bool display)
        {
            if (display)
            {
                SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfo\", \"data\": " + await ClientUtils.getPlayerInfo() + "}");
                SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"update\", \"type\": \"playerinfos\", \"data\": " + await ClientUtils.getPlayerInfos() + "}");
            }
            this._display = display;
            SetNuiFocus(display, display);
            SendNuiMessage("{\"ui\":\"BankNUI\", \"action\": \"show\", \"data\": \"" + display + "\"}");
        }

        private async Task onTick()
        {
            /*
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
            await Delay(0);
        }
    }
}
