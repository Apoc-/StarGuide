using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Behaviour.World
{
    public class ShipBehaviour : MonoBehaviour, IInitializable
    {
        public Dictionary<DeckCategory, List<DeckBehaviour>> Decks { get; } = new Dictionary<DeckCategory, List<DeckBehaviour>>();

        private void RegisterDeck(DeckBehaviour deck)
        {
            var cat = deck.DeckCategory;
            
            if (!Decks.ContainsKey(cat))
            {
                Decks.Add(cat, new List<DeckBehaviour>());
            }
            
            Decks[cat].Add(deck);
        }

        private void InitializeDecks()
        {
            var decks = GetComponentsInChildren<DeckBehaviour>();

            foreach (var deckBehaviour in decks)
            {
                deckBehaviour.Initialize();
                RegisterDeck(deckBehaviour);
            }
        }

        public void Initialize()
        {
            InitializeDecks();
        }
    }
}