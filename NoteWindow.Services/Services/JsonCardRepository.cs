using NoteWindow.Model.Enums;
using NoteWindow.Model.Model;
using NoteWindow.Services.Interfaces;
using System.Diagnostics;
using System.Text.Json;

namespace NoteWindow.Services.Services
{
    public class JsonCardRepository : ICardRepository
    {
        private readonly string _savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files/Dictionary/Cards/");
        private readonly string pathForNewCards;
        private string pathForReviewedCards;

        public JsonCardRepository() 
        {
            pathForNewCards = Path.Combine(_savePath, "New");
            pathForReviewedCards = Path.Combine(_savePath, "Reviewed");
            Directory.CreateDirectory(pathForNewCards);
            Directory.CreateDirectory(pathForReviewedCards);
        }

        public async Task SaveCardAsync(Card card)
        {
            if (!Directory.Exists(pathForNewCards))
            {
                Directory.CreateDirectory(pathForNewCards);
            }

            string json = JsonSerializer.Serialize(card);

            await File.WriteAllTextAsync(Path.Combine(pathForNewCards, card.Id + ".json"), json);
        }

        public async Task<IEnumerable<Card>> GetNewCardsAsync()
        {
            if (!Directory.Exists(pathForNewCards))
            {
                Directory.CreateDirectory(pathForNewCards);
            }

            IEnumerable<Card> newCards = await GetFilesAsync(pathForNewCards);

            return newCards;
        }

        public async Task<IEnumerable<Card>> GetReviewedCardsAsync()
        {
            if (!Directory.Exists(pathForReviewedCards))
            {
                Directory.CreateDirectory(pathForReviewedCards);
            }

            IEnumerable<Card> reviewedCards = await GetFilesAsync(pathForReviewedCards);

            return reviewedCards;
        }
        public async Task<bool> MoveToReviewedFolder(Guid cardId)
        {
            string sourceFile = Path.Combine(pathForNewCards, cardId.ToString() + ".json");
            string destinationFile = Path.Combine(pathForReviewedCards, cardId.ToString() + ".json");
            if (!File.Exists(sourceFile))
            {
                Debug.WriteLine($"Не удалось найти файл {cardId}");
                return await Task.FromResult(false);
            }
            else
            { 
                string cardFile = await File.ReadAllTextAsync(sourceFile);

                Card? card = JsonSerializer.Deserialize<Card>(cardFile);
                if (card == null)
                {
                    Debug.WriteLine("Не удалось десерилизовать json в Card");
                    return await Task.FromResult(false);
                }
                card.Status = CardStatus.Reviewed;
                string json = JsonSerializer.Serialize(card);

                await File.WriteAllTextAsync(destinationFile, json);

                File.Delete(sourceFile);

                return await Task.FromResult(true);
            }
        }

        public async Task<bool> DeleteReviewedCard(Guid cardId)
        {
            string sourceFile = Path.Combine(pathForReviewedCards, cardId.ToString() + ".json");
            if (!File.Exists(sourceFile))
            {
                Debug.WriteLine($"Не удалось найти файл {cardId}");
                return await Task.FromResult(false);
            }
            else
            {
                File.Delete(sourceFile);
                return await Task.FromResult(true);
            }
        }

        private async Task<IEnumerable<Card>> GetFilesAsync(string pathToFiles)
        {
            try
            {
                string[] files = Directory.GetFiles(pathToFiles, "*.json");
                List<Card> card = new(files.Length);
                string json;

                foreach (string file in files)
                {
                    try
                    {
                        json = await File.ReadAllTextAsync(file);
                        card.Add(JsonSerializer.Deserialize<Card>(json));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Не удалось загрузить {file}: {ex.Message}");
                        continue;
                    }
                }

                return card!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
