using Core.Application.Contracts.Features.Hotel.Commands.Create;
using Core.Application.Extensions;
using Core.Domain.Persistence.Contracts;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Hotel.Commands.Create
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Response<int>>
    {
        #region ctor
        private readonly IPersistenceUnitOfWork _persistenceUnitOfWork;
        private readonly ILogger<CreateHotelCommandHandler> _logger;
        private List<String> _validationError;

        public CreateHotelCommandHandler(IPersistenceUnitOfWork persistenceUnitOfWork, ILogger<CreateHotelCommandHandler> logger)
        {
            _persistenceUnitOfWork = persistenceUnitOfWork;
            _logger = logger;
            _validationError = new List<string>();
        }
        #endregion
        public async Task<Response<int>> Handle(CreateHotelCommand command, CancellationToken cancellationToken)
        {
            try
            {
                #region checking feature given or not 
                if (command.Features == null || !command.Features.Any())
                {
                    string msg = "Hotel features is required";
                    _logger.LogError(msg);
                    return Response<int>.Fail(msg);
                }
                #endregion

                var newHotel = new Domain.Persistence.Entities.Hotel
                {
                    Name = command.Name,
                    Description = command.Description,
                    Address = command.Address,
                    City = command.City,
                    State = command.State,
                    Country = command.Country,
                    Phone = command.Phone,
                    Email = command.Email,
                    Rating = command.Rating,
                    Features = command.Features == null ? null : command.Features.Select(s => new Domain.Persistence.Entities.HotelFeature
                    {
                        Name = s.Name,
                        Description = s.Description
                    }).ToList()
                };
                await _persistenceUnitOfWork.Hotel.AddAsync(newHotel);
                await _persistenceUnitOfWork.SaveChangesAsync();

                return Response<int>.Success(newHotel.Id, "Successfully created hotel.");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetFullMessage());
                _validationError.Add(ex.GetFullMessage());
                return Response<int>.Fail(_validationError);
            }
        }
    }
}
