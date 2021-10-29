using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// an enum defines a variable type with a few prenamed values
public enum eCardState {
    drawpile,
    tableau,
    target,
    discard
}

public class CardProspector : Card { //make sure CardProspector extends Card
    [Header("Set Dymanically: CardProspector")]
    //this is how you use the enum eCardState
    public eCardState state = eCardState.drawpile;
    // the hiddenBy list store which other cards will keep this one face down
    public List<CardProspector> hiddenBy = new List<CardProspector>();
    // LayoutID matches this card to the tableau XML if its a tableau card
    public int layoutID;
    // the SlotDef class stores information pulled in from the LayoutXML <slot>
    public SlotDef slotDef;

    // this allows the card to react to being clicked 
    override public void OnMouseUpAsButton() {
        // call the CardClicked method on teh prospector singleton
        Prospector.S.CardClicked(this);
        //also call the base cladd (card.cs) version of this method
        base.OnMouseUpAsButton();
    }
}