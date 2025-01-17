using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * An abstract class that all conversation cards override/inherit from
 * All of a cards atrributes and information will be stored in a dictionary which can only be accessed through getter methods.
 * The values for the cards attributes are declared in derived card classes.
 */


/* The card class's non-default Patience and Compliance properties automatically update their linked frontend card*/
public abstract class Card
{

    protected readonly int _id;
    protected IDictionary<string, string> _metadata = new Dictionary<string, string>();
    protected CardPrefabController frontendController;
    protected int position;

    protected Dictionary<string, IExecutableEffect> __localEffects = new();

    public int DefaultCompliance { get { return int.Parse(_metadata["compliance"]); } }

    public string DefaultElement { get { return _metadata["element"]; } }
    


    public float StackableComplianceMod 
    {
        get { return _stackableComplianceMod; }
        set 
        {
            _stackableComplianceMod = value;
            frontendController.SetCompliance(GetTotalCompliance());
        } 
    }  // Note! Stackable values are ALWAYS a multiplication of default. No additions
    private float _stackableComplianceMod = 0;
    public int UnstackableComplianceMod
    {
        get { return _unStackableComplianceMod; }
        set
        {
            _unStackableComplianceMod = value;
            frontendController.SetCompliance(GetTotalCompliance());
        }
    }  // Note! Unstackable values are ALWAYS additions. No multiplications
    private int _unStackableComplianceMod = 0;

    public bool ElementOverridden
    {
        get { return _elementOverriden; }
        set
        {
            _elementOverriden = value;
            frontendController.SetCompliance(GetTotalCompliance());
            frontendController.SetPatience(GetTotalPatience());
        }
    }  // Note! Override is for fully overrulling an element value
    private bool _elementOverriden = false;

    public string ElementOverride
    {
        get { return _elementOverride; }
        set
        {
            _elementOverride = value;
            frontendController.SetCompliance(GetTotalCompliance());
            frontendController.SetPatience(GetTotalPatience());
        }
    }  // Note! Override is for fully overrulling an element value 
    private string _elementOverride = "";

    public int ComplianceOverride
    {
        get { return _complianceOverride; }
        set
        {
            _complianceOverride = value;
            frontendController.SetCompliance(GetTotalCompliance());
        }
    }  // Note! Override is for fully overrulling a compliance value (e.g. the next card you play will give no compliance)
    private int _complianceOverride = 0; 
    public bool ComplianceOverridden
    {
        get { return _complianceOverriden; }
        set
        {
            _complianceOverriden = value;
            frontendController.SetCompliance(GetTotalCompliance());
        }
    }  // Note! Override is for fully overrulling a compliance value (e.g. the next card you play will give no compliance)
    private bool _complianceOverriden = false;

    public int DefaultPatience { get { return int.Parse(_metadata["patience"]); } }
    public float StackablePatienceMod
    {
        get { return _stackablePatienceMod; }
        set
        {
            _stackablePatienceMod = value;
            frontendController.SetPatience(GetTotalPatience());
        }
    }  // Note! Stackable values are ALWAYS a multiplication of default. No additions
    private float _stackablePatienceMod = 0;   
    public int UnstackablePatienceMod
    {
        get { return _unStackablePatienceMod; }
        set
        {
            _unStackablePatienceMod = value;
            frontendController.SetPatience(GetTotalPatience());
        }
    }  // Note! Unstackable values are ALWAYS additions. No multiplications
    private int _unStackablePatienceMod = 0;     
    public int PatienceOverride
    {
        get { return _patienceOverride; }
        set
        {
            _patienceOverride = value;
            frontendController.SetPatience(GetTotalPatience());
        }
    }  // Note! Override is for fully overrulling a patience value (e.g. the next card you play will cost no patience)
    private int _patienceOverride = 0;      
    public bool PatienceOverridden
    {
        get { return _patienceOverriden; }
        set
        {
            _patienceOverriden = value;
            frontendController.SetPatience(GetTotalPatience());
        }
    }  // Note! Override is for fully overrulling a patience value (e.g. the next card you play will cost no patience)
    private bool _patienceOverriden = false;

    public Card(int id)
    {
        this._id = id;
    }
    
    public void DisplayEffect(IExecutableEffect effect)
    {
        frontendController.DisplayEffect(effect);
    }

    public void ClearVisualEffects()
    {
        frontendController.ClearEffects();
    }

    public void Clear()
    {
        StackableComplianceMod = 0;
        UnstackableComplianceMod = 0;
        StackablePatienceMod = 0;
        UnstackableComplianceMod = 0;
        PatienceOverridden = false;
        PatienceOverride = 0;
        ComplianceOverridden = false;
        ComplianceOverride = 0;
        ElementOverridden = false;
        ElementOverride = "";
        ClearVisualEffects();
    }

    public int GetId() { return this._id; }
    public string GetElement() { return _metadata["element"]; }
    public string GetName() { return _metadata["name"]; }
    public string GetDescription() { return _metadata["description"]; }

    // for play executions
    public virtual void OnPlay()
    {
        Debug.Log("Card " + GetName() + " had it's base OnPlay called");
        // this is where triggering an effect may go
    }

    public virtual void OnChange()
    {
        Debug.Log("Card " + GetName() + " had it's base OnChange called");
        // this is where triggering an effect may go
    }

    public virtual void OnDraw()
    {
        Debug.Log("Card " + GetName() + " had it's base OnDraw called");
        // this is where triggering an effect may go
    }

    public int GetTotalCompliance() 
    {
        if (!ComplianceOverridden)
        {
            return DefaultCompliance + (int)(DefaultCompliance * StackableComplianceMod) + UnstackableComplianceMod;
        }
        else
        {
            return ComplianceOverride;
        }
        
    }
    public int GetTotalPatience() 
    { 
        if (!PatienceOverridden) 
        {
            return DefaultPatience + (int)(DefaultPatience * StackablePatienceMod) + UnstackablePatienceMod;
        }
        else
        {
            return PatienceOverride;
        }
        
    }

    public int GetPosition() { return position; }
    public void SetPosition(int index) { position = index; }
//    public void Execute() { GameState.Meta.activeEncounter.Value.AddFilter(int.Parse(_metadata["duration"]), int.Parse(_metadata["filterId"])); }
    public void SetAndInitializeFrontendController(CardPrefabController controller)
    {
        frontendController = controller;
        frontendController.SetCardName(GetName());
        frontendController.SetCardDescription(GetDescription());
        frontendController.SetDefaultPatience(DefaultPatience);
        frontendController.SetPosition(GetPosition());
        frontendController.SetElement(GetElement());
        frontendController.SetBackground(GetId());
        if (GetElement() != "Preparation")
        {
            frontendController.SetDefaultCompliance(DefaultCompliance);
        }
        Debug.Log("Ran Card Initialization");
    }
    public CardPrefabController GetFrontendController()
    {
        return frontendController;
    }

    public void SetAndInitializeNoEncounterFrontendController(NoEncounterCardPrefabController controller)
    {
        controller.SetCardName(GetName());
        controller.SetCardId(GetId());
        controller.SetCardDescription(GetDescription());
        controller.SetDefaultPatience(DefaultPatience);
        controller.SetElement(GetElement());
        controller.SetBackground(GetId());
        if (GetElement() != "Preparation")
        {
            controller.SetDefaultCompliance(DefaultCompliance);
        }
        Debug.Log("Ran Card Initialization");
    }
}

/*
 * NOTE: Antiquated
 * 
 * An abstract class that all preparation cards override/inherit from
 * All of a cards atrributes and information will be stored in a dictionary which can only be accessed through getter methods.
 * The values for the cards attributes are declared in derived card classes.
 */
public abstract class PreparationCard
{

    protected readonly int _id;
    protected IDictionary<string, string> _metadata = new Dictionary<string, string>();
    protected List<Action> methods = new List<Action>();

    public PreparationCard(int id)
    {
        this._id = id;


    }

    // some getters
    public int GetId() { return this._id; }
    public string GetName() { return _metadata["name"]; }
    public string GetDescription() { return _metadata["description"]; }
    public string GetCost() { return _metadata["cost"]; }

}