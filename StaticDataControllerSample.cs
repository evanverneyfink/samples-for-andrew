using System.ComponentModel.DataAnnotations;

public interface ICreateRequest<T>
{
    IDictionary<string, string> Validate();

    T ToEntity();
}

public class EpisodeCreateRequest : ICreateRequest<Episode>
{
    [Required]
    public int ShowId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    public Episode ToEntity()
    {
        return new Episode
        {
            ShowId = ShowId,
            Name = Name
        };
    }
}

[ApiController]
public abstract class StaticDataController<TEntity, TCreateRequest> : Controller
    where TEntity : EntityWithId, new()
    where TCreateRequest : ICreateRequest<TEntity>
{
    protected ModelState ModelState { get; }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(TCreateRequest createRequest)
    {
        Validate(createRequest);

        if (ModelState.IsValid)
            return ValidationProblem();

        Repository.Add(createRequest.ToEntity());
    }

    protected virtual void Validate(TCreateRequest createRequest)
    {
    }
}

public class EpisodeController : StaticDataController<Episode, EpisodeCreateRequest>
{
}