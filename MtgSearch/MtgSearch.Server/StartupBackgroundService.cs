

using MtgSearch.Server.Models.Logic;

namespace MtgSearch.Server
{
    public class StartupBackgroundService: BackgroundService
    {
        private readonly ICardRepository cardRepo;
        public StartupBackgroundService(ICardRepository cardRepo)
        {
            this.cardRepo = cardRepo;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await cardRepo.Update(stoppingToken);
            await cardRepo.Initialize(stoppingToken);
        }
    }
}
