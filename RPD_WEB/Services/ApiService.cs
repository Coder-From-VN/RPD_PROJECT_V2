using RPD_WEB.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly LocalStorageService _storage;

    public ApiService(HttpClient http, LocalStorageService storage)
    {
        _http = http;
        _storage = storage;
    }
    //AB
    private async Task SetAuthHeader()
    {
        var token = await _storage.GetItem("token");

        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<PagedResult<Ability>> GetAbilities(int page = 1, string search = "")
    {
        await SetAuthHeader();

        return await _http.GetFromJsonAsync<PagedResult<Ability>>(
            $"/api/Abilities?PageNumber={page}&PageSize=10&Search={search}"
        );
    }

    public async Task CreateAbility(Ability item)
    {
        await SetAuthHeader();
        await _http.PostAsJsonAsync("/api/Abilities", item);
    }

    public async Task UpdateAbility(Ability item)
    {
        await SetAuthHeader();
        await _http.PutAsJsonAsync($"/api/Abilities/{item.AbID}", item);
    }

    public async Task DeleteAbility(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"/api/Abilities/{id}");
    }

    public async Task UploadFile(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("/api/Abilities/upload", content);
    }

    //EG
    public async Task<PagedResult<EggGroup>> GetEggGroups(int page = 1, string search = "")
    {
        await SetAuthHeader();

        return await _http.GetFromJsonAsync<PagedResult<EggGroup>>(
            $"/api/EggGroup?PageNumber={page}&PageSize=10&Search={search}"
        );
    }

    public async Task CreateEggGroup(EggGroup item)
    {
        await SetAuthHeader();
        await _http.PostAsJsonAsync("/api/EggGroup", item);
    }

    public async Task UpdateEggGroup(EggGroup item)
    {
        await SetAuthHeader();
        await _http.PutAsJsonAsync($"/api/EggGroup/{item.EgID}", item);
    }

    public async Task DeleteEggGroup(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"/api/EggGroup/{id}");
    }

    public async Task UploadFileEG(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("/api/EggGroup/upload", content);
    }

    //GV
    public async Task<PagedResult<GameVersion>> GetGameVersions(int page = 1, string search = "")
    {
        await SetAuthHeader();

        return await _http.GetFromJsonAsync<PagedResult<GameVersion>>(
            $"/api/GameVersion?PageNumber={page}&PageSize=10&Search={search}"
        );
    }

    public async Task CreateGameVersion(GameVersion item)
    {
        await SetAuthHeader();

        await _http.PostAsJsonAsync("/api/GameVersion", new
        {
            gvName = item.GvName,
            gvGen = item.GvGen
        });
    }

    public async Task UpdateGameVersion(GameVersion item)
    {
        await SetAuthHeader();

        await _http.PutAsJsonAsync($"/api/GameVersion/{item.GvID}", new
        {
            gvName = item.GvName,
            gvGen = item.GvGen
        });
    }

    public async Task DeleteGameVersion(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"/api/GameVersion/{id}");
    }

    public async Task UploadFileGV(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("/api/GameVersion/upload", content);
    }

    //GR
    public async Task<PagedResult<GrowthRate>> GetGrowthRates(int page = 1, string search = "")
    {
        await SetAuthHeader();

        return await _http.GetFromJsonAsync<PagedResult<GrowthRate>>(
            $"/api/GrowthRate?PageNumber={page}&PageSize=10&Search={search}"
        );
    }

    public async Task CreateGrowthRate(GrowthRate item)
    {
        await SetAuthHeader();

        await _http.PostAsJsonAsync("/api/GrowthRate", new
        {
            grName = item.GrName,
            grTotalExp = item.GrTotalExp
        });
    }

    public async Task UpdateGrowthRate(GrowthRate item)
    {
        await SetAuthHeader();

        await _http.PutAsJsonAsync($"/api/GrowthRate/{item.GrowthRateID}", new
        {
            grName = item.GrName,
            grTotalExp = item.GrTotalExp
        });
    }

    public async Task DeleteGrowthRate(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"/api/GrowthRate/{id}");
    }

    public async Task UploadFileGR(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("/api/GrowthRate/upload", content);
    }

    //MOVE
    public async Task<PagedResult<Move>> GetMoves(int page = 1, string search = "")
    {
        await SetAuthHeader();

        return await _http.GetFromJsonAsync<PagedResult<Move>>(
            $"api/Move?PageNumber={page}&PageSize=10&Search={search}"
        );
    }

    public async Task CreateMove(Move item)
    {
        await SetAuthHeader();

        await _http.PostAsJsonAsync("api/Move", new
        {
            moveName = item.MoveName,
            moveDamageClass = item.MoveDamageClass,
            movePower = item.MovePower,
            moveAccuracy = item.MoveAccuracy,
            movePP = item.MovePP,
            movePriority = item.MovePriority,
            moveDescription = item.MoveDescription,
            typesID = item.TypesID
        });
    }

    public async Task UpdateMove(Move item)
    {
        await SetAuthHeader();

        await _http.PutAsJsonAsync($"api/Move/{item.MoveID}", new
        {
            moveName = item.MoveName,
            moveDamageClass = item.MoveDamageClass,
            movePower = item.MovePower,
            moveAccuracy = item.MoveAccuracy,
            movePP = item.MovePP,
            movePriority = item.MovePriority,
            moveDescription = item.MoveDescription,
            typesID = item.TypesID
        });
    }

    public async Task DeleteMove(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"api/Move/{id}");
    }

    public async Task UploadFileMove(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("/api/Move/upload", content);
    }

    //ST
    public async Task<List<StatType>> GetStatTypes()
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<List<StatType>>("api/StatType");
    }

    public async Task CreateStatType(StatType item)
    {
        await SetAuthHeader();

        await _http.PostAsJsonAsync("api/StatType", new
        {
            stName = item.StName
        });
    }

    public async Task UpdateStatType(StatType item)
    {
        await SetAuthHeader();

        await _http.PutAsJsonAsync($"api/StatType/{item.StID}", new
        {
            stName = item.StName
        });
    }

    public async Task DeleteStatType(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"api/StatType/{id}");
    }

    public async Task UploadFileStatType(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("/api/StatType/upload", content);
    }

    //Type
    public async Task<List<Types>> GetTypes()
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<List<Types>>("api/Type");
    }

    public async Task CreateType(Types item)
    {
        await SetAuthHeader();

        await _http.PostAsJsonAsync("api/Type", new
        {
            typesName = item.TypesName
        });
    }

    public async Task UpdateType(Types item)
    {
        await SetAuthHeader();

        await _http.PutAsJsonAsync($"api/Type/{item.TypesID}", new
        {
            typesName = item.TypesName
        });
    }

    public async Task DeleteType(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"api/Type/{id}");
    }

    public async Task UploadTypeFile(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        await _http.PostAsync("api/Type/upload", content);
    }

    //pokemon
    public async Task<PagedResult<PokemonListDto>> GetPokemons(int page, string search)
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<PagedResult<PokemonListDto>>
            ($"/api/Pokemons?PageNumber={page}&PageSize=10&Search={search}");
    }

    public async Task<PokemonDetailDto> GetPokemonById(Guid id)
    {
        await SetAuthHeader();
        return await _http.GetFromJsonAsync<PokemonDetailDto>($"/api/Pokemons/{id}");
    }

    public async Task CreatePokemon(PostFullPokemonsDTO model)
    {
        await SetAuthHeader();
        await _http.PostAsJsonAsync("/api/Pokemons", model);
    }

    public async Task UpdatePokemon(Guid id, PutFullPokemonsDTO model)
    {
        await SetAuthHeader();
        await _http.PutAsJsonAsync($"/api/Pokemons/{id}", model);
    }

    public async Task DeletePokemon(Guid id)
    {
        await SetAuthHeader();
        await _http.DeleteAsync($"/api/Pokemons/{id}");
    }

    public async Task UploadPokemon(Stream fileStream, string fileName)
    {
        await SetAuthHeader();

        var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "pokemonFile", fileName);

        await _http.PostAsync("/api/Pokemons/upload", content);
    }
}