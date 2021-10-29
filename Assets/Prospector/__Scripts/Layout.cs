using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the SlotDef class is not a subclass of MonoBehavior, so it doesn't need a seperate C# script

[System.Serializable] // this makes SlotDefs visible in the Unity Inspector pane
public class SlotDef {
    public float x;
    public float y;
    public bool faceUp = false;
    public string layerName = "Default";
    public int layerID = 0;
    public int id;
    public List<int> hiddenBy = new List<int>();
    public string type = "slot";
    public Vector2 stagger;
}

public class Layout : MonoBehaviour {
    public PT_XMLReader xmlr; // just like deck, this has a PT_XMLReader
    public PT_XMLHashtable xml; // this variable is for faster xml access
    public Vector2 multiplier; //the offset of the tableau's center

    // SlotDef references
    public List<SlotDef> slotDefs; //All the SlotDefs for Row0 - Row3
    public SlotDef drawPile; 
    public SlotDef discardPile;

    //This holds all the possible names for the layers set by layerID
    public string[] sortingLayerNames = new string[] {"Row0", "Row1", "Row2", "Row3", "Discard", "Draw"};

    public void ReadLayout(string xmlText) {
        xmlr = new PT_XMLReader();
        xmlr.Parse(xmlText); //the XML is parsed
        xml = xmlr.xml["xml"][0]; //amd xml is set as a shortcut to the XML

        // read in the multiplier, which sets card spacing
        multiplier.x = float.Parse(xml["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml["multiplier"][0].att("y"));

        //read in the slots
        SlotDef tSD;
        //slotsX is used as a shortcut to all the <slot>s
        PT_XMLHashList slotsX = xml["slot"];

        for (int i = 0; i < slotsX.Count; i++) {
            tSD = new SlotDef(); //Create a new SlotDef instance
            if (slotsX[i].HasAtt("type")) {
                //if this <slot> has a type attribute parse it
                tSD.type = slotsX[i].att("type");
            } else {
                //if not, set its tyoe to "slot"; its a card in the rows
                tSD.type = "slot";
            }

            //Various attributes are parsed into numerical values
            tSD.x = float.Parse(slotsX[i].att("x"));
            tSD.y = float.Parse(slotsX[i].att("y"));
            tSD.layerID = int.Parse(slotsX[i].att("layer"));
            //this converts the number of the LayerID into the text layerName
            tSD.layerName = sortingLayerNames[tSD.layerID];

            switch(tSD.type) {
                //pull additional attributes based on the tyoe of this <slot>
                case "slot":
                    tSD.faceUp = (slotsX[i].att("faceup") == "1");
                    tSD.id = int.Parse(slotsX[i].att("id"));
                    if (slotsX[i].HasAtt("hiddenby")) { // a method to convert a string to two numbers. The hiddenBy in the slot id is a string (Ex. "18, 19").
                        string[] hiding = slotsX[i].att("hiddenby").Split(','); // this is to seperate the string to two strings being seperated by the ",". (Ex. ["18"] ["19"]).
                        foreach (string s in hiding) { 
                            tSD.hiddenBy.Add(int.Parse(s)); // convert the two strings into integers.
                        }
                    }
                    slotDefs.Add(tSD);
                    break;

                case "drawpile":
                    tSD.stagger.x = float.Parse(slotsX[i].att("xstagger"));
                    drawPile = tSD;
                    break;

                case "discardpile":
                    discardPile = tSD;
                    break;
            }
        }
    }

}
