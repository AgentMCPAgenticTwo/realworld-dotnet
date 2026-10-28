using CleanArchitecture.Application.Tutorials.Commands.CreateTutorial;
using CleanArchitecture.Application.Tutorials.Commands.DeleteAllTutorials;
using CleanArchitecture.Application.Tutorials.Commands.DeleteTutorial;
using CleanArchitecture.Application.Tutorials.Commands.UpdateTutorial;
using CleanArchitecture.Application.Tutorials.Queries.GetTutorial;
using CleanArchitecture.Application.Tutorials.Queries.ListTutorials;
using CleanArchitecture.Contracts.Tutorials;
using CleanArchitecture.Domain.Tutorials;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[AllowAnonymous]
[Route("api/tutorials")]
public class TutorialsController(ISender _mediator) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> ListTutorials([FromQuery] string? title)
    {
        var query = new ListTutorialsQuery(title);

        var result = await _mediator.Send(query);

        return result.Match(
            tutorials => Ok(tutorials.ConvertAll(ToDto)),
            Problem);
    }

    [HttpGet("{tutorialId:guid}")]
    public async Task<IActionResult> GetTutorial(Guid tutorialId)
    {
        var query = new GetTutorialQuery(tutorialId);

        var result = await _mediator.Send(query);

        return result.Match(
            tutorial => Ok(ToDto(tutorial)),
            Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTutorial(CreateTutorialRequest request)
    {
        var command = new CreateTutorialCommand(
            request.Title,
            request.Description,
            request.Published,
            request.CategoryId);

        var result = await _mediator.Send(command);

        return result.Match(
            tutorial => CreatedAtAction(
                actionName: nameof(GetTutorial),
                routeValues: new { TutorialId = tutorial.Id },
                value: ToDto(tutorial)),
            Problem);
    }

    [HttpPut("{tutorialId:guid}")]
    public async Task<IActionResult> UpdateTutorial(Guid tutorialId, UpdateTutorialRequest request)
    {
        var command = new UpdateTutorialCommand(
            tutorialId,
            request.Title,
            request.Description,
            request.Published,
            request.CategoryId);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => Ok(new MessageResponse("Tutorial was updated successfully.")),
            Problem);
    }

    [HttpDelete("{tutorialId:guid}")]
    public async Task<IActionResult> DeleteTutorial(Guid tutorialId)
    {
        var command = new DeleteTutorialCommand(tutorialId);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllTutorials()
    {
        var command = new DeleteAllTutorialsCommand();

        var result = await _mediator.Send(command);

        return result.Match(
            count => Ok(new MessageResponse($"{count} Tutorials were deleted successfully.")),
            Problem);
    }

    private TutorialResponse ToDto(Tutorial tutorial) =>
        new(
            tutorial.Id,
            tutorial.Title,
            tutorial.Description,
            tutorial.Published,
            tutorial.CategoryId,
            tutorial.CreatedAt,
            tutorial.UpdatedAt);
}
