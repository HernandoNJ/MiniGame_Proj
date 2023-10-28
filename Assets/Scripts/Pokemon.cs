using System;
using System.Collections.Generic;

namespace Pokemon.API
{
	[Serializable]
	public class Pokemon
	{
		public List<Abilities> abilities { get; set; }
		public int base_experience { get; set; }
		public List<Forms> forms { get; set; }
		public List<GameIndices> game_indices { get; set; }
		public int height { get; set; }
		public List<HeldItems> held_items { get; set; }
		public int id { get; set; }
		public bool is_default { get; set; }
		public string location_area_encounters { get; set; }
		public List<Moves> moves { get; set; }
		public string name { get; set; }
		public int order { get; set; }
		public List<PastAbilities> past_abilities;
		public List<PastTypes> past_types;
		public Species species { get; set; }
		public Sprites sprites { get; set; }
		public List<Stats> stats { get; set; }
		public List<Types> types { get; set; }
		public int weight { get; set; }
	}

	[Serializable]
	public class PastAbilities { }

	[Serializable]
	public class PastTypes { }

	[Serializable]
	public class Chain
	{
		public List<object> evolution_details { get; set; }
		public List<EvolvesTo> evolves_to { get; set; }
		public bool is_baby { get; set; }
		public Species species { get; set; }
	}

	[Serializable]
	public class EvolutionDetail
	{
		public object gender { get; set; }
		public object held_item { get; set; }
		public object item { get; set; }
		public object known_move { get; set; }
		public object known_move_type { get; set; }
		public object location { get; set; }
		public object min_affection { get; set; }
		public object min_beauty { get; set; }
		public object min_happiness { get; set; }
		public int min_level { get; set; }
		public bool needs_overworld_rain { get; set; }
		public object party_species { get; set; }
		public object party_type { get; set; }
		public object relative_physical_stats { get; set; }
		public string time_of_day { get; set; }
		public object trade_species { get; set; }
		public Trigger trigger { get; set; }
		public bool turn_upside_down { get; set; }
	}

	[Serializable]
	public class EvolvesTo
	{
		public List<EvolutionDetail> evolution_details { get; set; }
		public List<object> evolves_to { get; set; }
		public bool is_baby { get; set; }
		public Species species { get; set; }
	}

	[Serializable]
	public class Species
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Trigger
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class PokemonResults
	{
		public int count { get; set; }
		public string next { get; set; }
		public object previous { get; set; }
		public List<Result> results { get; set; }
	}

	[Serializable]
	public class Result
	{
		public string name { get; set; }
		public string url { get; set; }
	}


	[Serializable]
	public class Stats
	{
		public int base_stat { get; set; }
		public int effort { get; set; }
		public Stat stat { get; set; }
	}

	[Serializable]
	public class Stat
	{
		public string name { get; set; }
		public string url { get; set; }
	}

		[Serializable]
	public class Sprites
	{
		public string back_default { get; set; }
		public string back_female { get; set; }
		public string back_shiny { get; set; }
		public string back_shiny_female { get; set; }
		public string front_default { get; set; }
		public string front_female { get; set; }
		public string front_shiny { get; set; }
		public string front_shiny_female { get; set; }
	}

	[Serializable]
	public class Types
	{
		public int slot { get; set; }
		public Type type { get; set; }
	}

	[Serializable]
	public class Type
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class PokeTypePast
	{
		public Ability generation { get; set; }
		public List<Types> types { get; set; }
	}

	[Serializable]
	public class Moves
	{
		public Move move { get; set; }
		public List<VersionGroupDetails> version_group_details { get; set; }
	}

	[Serializable]
	public class Move
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class VersionGroupDetails
	{
		public int level_learned_at { get; set; }
		public MoveLearnMethod move_learn_method { get; set; }
		public VersionGroup version_group { get; set; }
	}

	[Serializable]
	public class Abilities
	{
		public Ability ability { get; set; }
		public bool is_hidden { get; set; }
		public int slot { get; set; }
	}

	[Serializable]
	public class GameIndices
	{
		public int game_index { get; set; }
		public Version version { get; set; }
	}

	[Serializable]
	public class HeldItems
	{
		public Item item { get; set; }
		public List<VersionDetails> version_details { get; set; }
	}

	[Serializable]
	public class VersionDetails
	{
		public int rarity { get; set; }
		public Version version { get; set; }
	}

	[Serializable]
	public class Ability
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Item
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class MoveLearnMethod
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class VersionGroup
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Forms
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Color
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class EggGroup
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class EvolutionChain
	{
		public string url { get; set; }
	}

	[Serializable]
	public class EvolvesFromSpecies
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class FlavorTextEntry
	{
		public string flavor_text { get; set; }
		public Language language { get; set; }
		public Version version { get; set; }
	}

	[Serializable]
	public class Genera
	{
		public string genus { get; set; }
		public Language language { get; set; }
	}

	[Serializable]
	public class Generation
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class GrowthRate
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Habitat
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Language
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Name
	{
		public Language language { get; set; }
		public string name { get; set; }
	}

	[Serializable]
	public class PalParkEncounterArea
	{
		public PalParkArea area { get; set; }
		public int base_score { get; set; }
		public int rate { get; set; }
	}

	[Serializable]
	public class PokemonEncounter
	{
		public int base_score { get; set; }
		public PokemonSpecies pokemon_species { get; set; }
		public int rate { get; set; }
	}

	[Serializable]
	public class PalParkArea
	{
		public int id { get; set; }
		public string name { get; set; }
		public List<Name> names { get; set; }
		public List<PalParkEncounterSpecies> pokemon_encounters { get; set; }
	}

	[Serializable]
	public class PalParkEncounterSpecies
	{
		public int base_score { get; set; }
		public int rate { get; set; }
		public PokemonSpecies pokemon_species { get; set; }
	}

	[Serializable]
	public class Pokedex
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class PokedexNumber
	{
		public int entry_number { get; set; }
		public Pokedex pokedex { get; set; }
	}

	[Serializable]
	public class PokemonBasic
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class PokemonSpecies
	{
		public int Base_happiness { get; set; }
		public int Capture_rate { get; set; }
		public Color Color { get; set; }
		public List<EggGroup> egg_groups { get; set; }
		public EvolutionChain evolution_chain { get; set; }
		public EvolvesFromSpecies evolves_from_species { get; set; }
		public List<FlavorTextEntry> flavor_text_entries { get; set; }
		public List<object> form_descriptions { get; set; }
		public bool forms_switchable { get; set; }
		public int gender_rate { get; set; }
		public List<Genera> genera { get; set; }
		public Generation generation { get; set; }
		public GrowthRate growth_rate { get; set; }
		public Habitat habitat { get; set; }
		public bool has_gender_differences { get; set; }
		public int hatch_counter { get; set; }
		public int id { get; set; }
		public bool is_baby { get; set; }
		public bool is_legendary { get; set; }
		public bool is_mythical { get; set; }
		public string name { get; set; }
		public List<Name> names { get; set; }
		public int order { get; set; }
		public List<PalParkEncounterArea> pal_park_encounters { get; set; }
		public List<PokedexNumber> pokedex_numbers { get; set; }
		public Shape shape { get; set; }
		public List<Variety> varieties { get; set; }
	}

	[Serializable]
	public class Shape
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Variety
	{
		public bool is_default { get; set; }
		public Pokemon pokemon { get; set; }
	}

	[Serializable]
	public class Version
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class EvolutionChainRoot
	{
		public object baby_trigger_item { get; set; }
		public Chain chain { get; set; }
		public int id { get; set; }
	}

	// GetDataFromApi()
	//var newPokUrl = "https://pokeapi.co/api/v2/evolution-chain/300";

	//using (UnityWebRequest request = UnityWebRequest.Get(newPokUrl))
	//{
	//	yield return request.SendWebRequest();

	//	if (request.result == UnityWebRequest.Result.Success)
	//	{
	//		//PokeApiObj pokApi = JsonConvert.DeserializeObject<PokeApiObj>(request.downloadHandler.text);

	//	}
	//}





	/*
	 *  public class Root
    {
        public object baby_trigger_item { get; set; }
        public Chain chain { get; set; }
        public int id { get; set; }
    }
	 */


	//public class EvolutionChain
	//{

	//}
}