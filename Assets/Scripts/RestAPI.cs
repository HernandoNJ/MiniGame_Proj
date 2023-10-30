using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace Pokemon.API
{
    public class RestAPI : MonoBehaviour
    {
        public string initialPokemonResultsJson;
        public string pokeUrl = "https://pokeapi.co/api/v2/pokemon/300";
        public string pokemonResultsUrl = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=200";
        public string pokemonFromApiJson;
        public string rawImageUrl;
        public int pokeMaxCount;
        public int[] itemsCountArray;

        public GameObject pokeInfoPrefab;
        public GameObject newPokemon;
        public Texture2D newPokeTexture;

        public PokemonResults pokemonResults;
        public Pokemon pokemonFromApi;
        public PokemonSpecies pokemonSpecies;
        public EvolutionChain evolutionChain;
        public EvolutionChainRoot evolutionChainRoot;
        public ParentHandler[] parentHandlers = new ParentHandler[3];

        private UniTask _defaultUniTask;

        [SerializeField] private TMP_InputField outputArea;

        //pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon?offset=300&limit=200";
        //pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/300";
        //pokeApiV2Url = "https://pokeapi.co/api/v2/pokemon/ditto";

        private void Start()
        {
            _defaultUniTask = GetPokemonsData();
        }

        private async UniTask GetPokemonsData()
        {
            outputArea.text = "Loading...";
            await UniTask.Delay(1000);

            await GetPokemonResults();
            await GetPokemonFromAPI();
            await GetPokemonTexture();
            await GetPokemonSpecies();


        }
        private async Task GetPokemonResults()
        {
            //await GetNewObjectFromApi();

            initialPokemonResultsJson = (await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;
            pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);

            await SetOutputTextArea("Setting Pokemon results...", 500);
        }

        //private async Task GetNewObjectFromApi(string url, Object obj)
        //{
        //	initialPokemonResultsJson = (await UnityWebRequest.Get(pokemonResultsUrl).SendWebRequest()).downloadHandler.text;
        //	pokemonResults = JsonConvert.DeserializeObject<PokemonResults>(initialPokemonResultsJson);
        //}

        private async Task GetPokemonFromAPI()
        {
            pokemonFromApiJson = (await UnityWebRequest.Get(pokemonResults.results[0].url).SendWebRequest()).downloadHandler.text;
            pokemonFromApi = JsonConvert.DeserializeObject<Pokemon>(pokemonFromApiJson);

            await SetOutputTextArea("New Pokemon: " + pokemonFromApi.name, 500);
        }

        private async Task GetPokemonTexture()
        {
            rawImageUrl = pokemonFromApi.sprites.front_default;
            var pokemonTextureRequest = await UnityWebRequestTexture.GetTexture(rawImageUrl).SendWebRequest();
            newPokeTexture = ((DownloadHandlerTexture)pokemonTextureRequest.downloadHandler).texture;
            
            await SetOutputTextArea("Setting Pokemon Texture...", 500);
        }

        private async Task GetPokemonSpecies()
        {
            await SetOutputTextArea("Pokemon Species url: \n" + pokemonFromApi.species.url, 500);

            var pokemonSpeciesJson = (await UnityWebRequest.Get(pokemonFromApi.species.url).SendWebRequest()).downloadHandler.text;
            await SetOutputTextArea("Pokemon Species Json: " + "\n" + pokemonSpeciesJson, 500);

            pokemonSpecies = JsonConvert.DeserializeObject<PokemonSpecies>(pokemonSpeciesJson);
            await SetOutputTextArea("Pokemon Species: " + pokemonSpecies.ToString(), 500);

            await GetPokemonEvolutionChain();

        }
        private async Task GetPokemonEvolutionChain()
        {
            await SetOutputTextArea("Starting to get evolution chain... \n\n Evolution chain url: " + pokemonSpecies.evolution_chain.url, 1000);
            
            var evolutionChainJson = (await UnityWebRequest.Get(pokemonSpecies.evolution_chain.url).SendWebRequest()).downloadHandler.text;
            Debug.Log("Evolution chain url: " + pokemonSpecies.evolution_chain.url);
            Debug.Log("Evolution chain Json: \n" + evolutionChainJson);
            await SetOutputTextArea("Evolution chain Json: \n" + evolutionChainJson, 1000);

            // TODO check for bug here
            try
            {
                evolutionChainRoot = JsonConvert.DeserializeObject<EvolutionChainRoot>(evolutionChainJson);
            }
            catch (System.Exception exc)
            {
                outputArea.text = exc.Message + "\n*** Stack tracke: *** " + exc.StackTrace ;
                throw;
            }
            
            await SetOutputTextArea("Evolution chain root object: \n" + evolutionChainRoot.ToString(), 2000);
            await SetOutputTextArea("evol chain root - id: " + evolutionChainRoot.id + " - chain: " + evolutionChainRoot.chain, 2000);
        }

        private async Task SetOutputTextArea(string newText, int delayTime)
        {
            outputArea.text = newText;
            await UniTask.Delay(delayTime);
        }
    }
}


//		// Get evolution chain url
//		pokeSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species/" + pokemonFromApi.id;
//		using (UnityWebRequest request3 = UnityWebRequest.Get(pokeSpeciesUrl))
//		{


//private void SetNewPokemonPrefab(int i)
//{
//	newPokemon = Instantiate(pokeInfoPrefab);

//	newPokemon.GetComponent<PokemonInfo>().nameText.text = pokemonFromApi.name;
//	newPokemon.GetComponent<PokemonInfo>().exp.text = pokemonFromApi.base_experience.ToString();
//	newPokemon.GetComponent<PokemonInfo>().image.texture = newPokeTexture;

//	if (i == 0) newPokemon.GetComponent<PokemonInfo>().SetPokemonCardData();

//	if (i < itemsCountArray[0]) SetParentHandler(0);
//	else if (i < itemsCountArray[1]) SetParentHandler(1);
//	else if (i < itemsCountArray[2]) SetParentHandler(2);

//	void SetParentHandler(int index)
//	{
//		newPokemon.transform.SetParent(parentHandlers[index].transform);
//		parentHandlers[index].AddPokemonToList(newPokemon);
//	}
//}
