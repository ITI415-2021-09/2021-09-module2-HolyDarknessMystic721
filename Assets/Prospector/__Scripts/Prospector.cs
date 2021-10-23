using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Prospector : MonoBehaviour {

	static public Prospector 	S;

	[Header("Set in Inspector")]
	public TextAsset			deckXML;
	public TextAsset layoutXML;

	[Header("Set Dynamically")]
	public Deck					deck;
	public Layout layout;

	void Awake(){
		S = this;
	}

	void Start() {
		deck = GetComponent<Deck> (); // gets the deck
		deck.InitDeck (deckXML.text); //pass DeckXML to it
		Deck.Shuffle(ref deck.cards); // this shuffles the deck

		layout = GetComponent<Layout>();
		layout.ReadLayout(layoutXML.text);
	}

}
