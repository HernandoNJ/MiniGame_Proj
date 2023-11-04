using System;
using System.Collections.Generic;

namespace Pokemon.API
{
	[Serializable]
	public class Pokemon
	{
		public List<Abilities> abilities { get; set; }
		public int base_experience { get; set; }
		public List<BasicInfo> forms { get; set; }
		public List<GameIndices> game_indices { get; set; }
		public int height { get; set; }
		public List<HeldItems> held_items { get; set; }
		public int id { get; set; }
		public bool is_default { get; set; }
		public string location_area_encounters { get; set; }
		public List<Moves> moves { get; set; }
		public string name { get; set; }
		public int order { get; set; }
		//public List<PastAbilities> past_abilities;
		//public List<PastTypes> past_types;
		public BasicInfo species { get; set; }
		public Sprites sprites { get; set; }
		public List<Stats> stats { get; set; }
		public List<Types> types { get; set; }
		public int weight { get; set; }
	}

	[Serializable] public class PastAbilities { }

	[Serializable] public class PastTypes { }

	[Serializable]
	public class Chain
	{
		public List<object> evolution_details { get; set; }
		public List<Chain> evolves_to { get; set; }
		public bool is_baby { get; set; }
		public BasicInfo species { get; set; }
	}

	[Serializable]
	public class EvolutionDetails
	{
		public object gender { get; set; }
		public object held_item { get; set; }
		public BasicInfo item { get; set; }
		public object known_move { get; set; }
		public object known_move_type { get; set; }
		public object location { get; set; }
		public object min_affection { get; set; }
		public object min_beauty { get; set; }
		public object min_happiness { get; set; }
		public object min_level { get; set; }
		public bool needs_overworld_rain { get; set; }
		public object party_species { get; set; }
		public object party_type { get; set; }
		public object relative_physical_stats { get; set; }
		public string time_of_day { get; set; }
		public object trade_species { get; set; }
		public BasicInfo trigger { get; set; }
		public bool turn_upside_down { get; set; }
	}

	[Serializable]
	public class EvolvesTo
	{
		public List<EvolutionDetails> evolution_details { get; set; }
		public List<EvolvesTo> evolves_to { get; set; }
		public bool is_baby { get; set; }
		public BasicInfo species { get; set; }
	}

	[Serializable]
	public class PokemonResults
	{
		public int count { get; set; }
		public string next { get; set; }
		public object previous { get; set; }
		public List<BasicInfo> results { get; set; }
	}

	[Serializable]
	public class BasicInfo
	{
		public string name { get; set; }
		public string url { get; set; }
	}

	[Serializable]
	public class Stats
	{
		public int base_stat { get; set; }
		public int effort { get; set; }
		public BasicInfo stat { get; set; }
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
		public BasicInfo type { get; set; }
	}

	[Serializable]
	public class PokeTypePast
	{
		public BasicInfo generation { get; set; }
		public List<Types> types { get; set; }
	}

	[Serializable]
	public class Moves
	{
		public BasicInfo move { get; set; }
		public List<VersionGroupDetails> version_group_details { get; set; }
	}

	[Serializable]
	public class VersionGroupDetails
	{
		public int level_learned_at { get; set; }
		public BasicInfo move_learn_method { get; set; }
		public BasicInfo version_group { get; set; }
	}

	[Serializable]
	public class Abilities
	{
		public BasicInfo ability { get; set; }
		public bool is_hidden { get; set; }
		public int slot { get; set; }
	}

	[Serializable]
	public class GameIndices
	{
		public int game_index { get; set; }
		public BasicInfo version { get; set; }
	}

	[Serializable]
	public class HeldItems
	{
		public BasicInfo item { get; set; }
		public List<VersionDetails> version_details { get; set; }
	}

	[Serializable]
	public class VersionDetails
	{
		public int rarity { get; set; }
		public BasicInfo version { get; set; }
	}

	[Serializable]
	public class EvolutionChainUrl
	{
		public string url { get; set; }
	}

	[Serializable]
	public class FlavorTextEntry
	{
		public string flavor_text { get; set; }
		public BasicInfo language { get; set; }
		public BasicInfo version { get; set; }
	}

	[Serializable]
	public class Genera
	{
		public string genus { get; set; }
		public BasicInfo language { get; set; }
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
		public List<BasicInfo> names { get; set; }
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
	public class PokedexNumber
	{
		public int entry_number { get; set; }
		public BasicInfo pokedex { get; set; }
	}

	[Serializable]
	public class PokemonSpecies
	{
		public int Base_happiness { get; set; }
		public int Capture_rate { get; set; }
		public BasicInfo Color { get; set; }
		public List<BasicInfo> egg_groups { get; set; }
		public EvolutionChainUrl evolution_chain { get; set; }
		public BasicInfo evolves_from_species { get; set; }
		public List<FlavorTextEntry> flavor_text_entries { get; set; }
		public List<object> form_descriptions { get; set; }
		public bool forms_switchable { get; set; }
		public int gender_rate { get; set; }
		public List<Genera> genera { get; set; }
		public BasicInfo generation { get; set; }
		public BasicInfo growth_rate { get; set; }
		public BasicInfo habitat { get; set; }
		public bool has_gender_differences { get; set; }
		public int hatch_counter { get; set; }
		public int id { get; set; }
		public bool is_baby { get; set; }
		public bool is_legendary { get; set; }
		public bool is_mythical { get; set; }
		public string name { get; set; }
		public List<BasicInfo> names { get; set; }
		public int order { get; set; }
		public List<PalParkEncounterArea> pal_park_encounters { get; set; }
		public List<PokedexNumber> pokedex_numbers { get; set; }
		public BasicInfo shape { get; set; }
		public List<Variety> varieties { get; set; }
	}

	[Serializable]
	public class Variety
	{
		public bool is_default { get; set; }
		public Pokemon pokemon { get; set; }
	}

	[Serializable]
	public class EvolutionChainRoot
	{
		public object baby_trigger_item { get; set; }
		public Chain chain { get; set; }
		public int id { get; set; }
	}
}