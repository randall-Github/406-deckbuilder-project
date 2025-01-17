using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map : MonoBehaviour
{
    public void UpdateLocations()
    {
        try
        {
            if (GameState.Meta.currentDay.Value == 7)
            {
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Bar] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RailYard] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Boxcar] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BerryFarm] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BreakfastPalace] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.LumberYard] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Motel] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.PostOfficeInside] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RailYard] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RatMobCave] = false;

                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RealMainStreet] = true;

                return;
            }

            if (GameState.Meta.currentGameplayPhase.Value == GameState.Meta.GameplayPhases.Tutorial)
            {
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BerryFarm] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RealMainStreet] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Motel] = true;

                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Bar] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RailYard] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BreakfastPalace] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.LumberYard] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.PostOfficeInside] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RatMobCave] = false;
            }
            else if(GameState.Meta.currentGameplayPhase.Value == GameState.Meta.GameplayPhases.Phase_1)
            {
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BerryFarm] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BreakfastPalace] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.LumberYard] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Motel] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.PostOfficeInside] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RatMobCave] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RealMainStreet] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RailYard] = true;

                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Bar] = false;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Boxcar] = false;

            }
            else
            {
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Bar] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RailYard] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Boxcar] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BerryFarm] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.BreakfastPalace] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.LumberYard] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Motel] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.PostOfficeInside] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RailYard] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RatMobCave] = true;
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.RealMainStreet] = true;
            }
            if (GameState.NPCs.Big_Rat.encountersWon.Value + GameState.NPCs.Doug.encountersWon.Value 
            + GameState.NPCs.Elk.encountersWon.Value >= 2)
            {
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Boxcar] = true;
            }
            if (GameState.NPCs.Big_Rat.encountersWon.Value + GameState.NPCs.Doug.encountersWon.Value 
            + GameState.NPCs.Elk.encountersWon.Value >= 1)
            {
                GameState.Player.locationsViewable.Value[GameState.Player.Locations.Bar] = true;
            }
            GameState.Player.locationsViewable.Raise();

        }
        catch (MissingReferenceException e)
        {
            e.Message.Contains('e');
            GameState.Meta.currentAct.OnChange -= UpdateLocations;
            GameState.Meta.currentDay.OnChange -= UpdateLocations;
        }
        catch (NullReferenceException e)
        {
            e.Message.Contains('e');
            GameState.Meta.currentAct.OnChange -= UpdateLocations;
            GameState.Meta.currentDay.OnChange -= UpdateLocations;
        }
    }

    void Start()
    {
        UpdateLocations();
        GameState.Meta.currentAct.OnChange += UpdateLocations;
        GameState.Meta.currentDay.OnChange += UpdateLocations;
    }
}
