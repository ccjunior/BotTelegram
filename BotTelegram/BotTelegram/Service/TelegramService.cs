using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotTelegram.Service
{
    public class TelegramService : ITelegramService
    {
        private readonly string _token = "seu_token_aqui";
        static ITelegramBotClient botClient;

        public async void BotHandler()
        {
            botClient = new TelegramBotClient(_token);

            var me = await botClient.GetMeAsync();
            Console.Title = me.Username;

            botClient.OnMessage += BotOnMessageReceived;
            botClient.OnMessageEdited += BotOnMessageReceived;
            botClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            botClient.OnReceiveError += BotOnReceiveError;

            botClient.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            Console.ReadLine();
            botClient.StopReceiving();


        }

        public void SendMessage()
        {
            throw new NotImplementedException();
        }


        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            switch (message.Text.Split(' ').First())
            {
                // mostra ooções na linha
                case "/inline":
                    await SendInlineKeyboard(message);
                    break;

                // mostra um teclado personalizado com opções
                case "/keyboard":
                    await SendReplyKeyboard(message);
                    break;

                // envia uma foto
                case "/photo":
                    await SendDocument(message);
                    break;

                // pede pra escolher entre enviar o contato ou a localização
                case "/request":
                    await RequestContactAndLocation(message);
                    break;

                // Envia um contato estático com card
                case "/contactFull":
                    await SendStaticFullContact(message);
                    break;

                // Envia um contato estático simples
                case "/contactSimple":
                    await SendStaticSimpleContact(message);
                    break;

                // Envia a localização estática completa
                case "/locationFull":
                    await SendStaticFullLocation(message);
                    break;

                // Envia a localização estática completa
                case "/locationSimple":
                    await SendStaticLocationSimple(message);
                    break;

                // Envia uma mensagem sobre o bot
                case "/about":
                    await AboutBot(message);
                    break;

                default:
                    await Usage(message);
                    break;
            }

            static async Task SendInlineKeyboard(Message message)
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                // Simulate longer running task
                await Task.Delay(500);

                var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                        // first row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("1.1", "11"),
                            InlineKeyboardButton.WithCallbackData("1.2", "12"),
                        },
                        // second row
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("2.1", "21"),
                            InlineKeyboardButton.WithCallbackData("2.2", "22"),
                        }
                    });
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: inlineKeyboard
                );
            }

            static async Task SendReplyKeyboard(Message message)
            {
                var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[][]
                    {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                    },
                    resizeKeyboard: true
                );

                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Choose",
                    replyMarkup: replyKeyboardMarkup

                );
            }

            static async Task SendDocument(Message message)
            {
                await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

                const string filePath = @"C:\Users\eriks\Desktop\erik.jpg";
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
                await botClient.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: new InputOnlineFile(fileStream, fileName),
                    caption: "Bonitão você hein!"
                );
            }

            static async Task RequestContactAndLocation(Message message)
            {
                var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                });
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Who or Where are you?",
                    replyMarkup: RequestReplyKeyboard
                );
            }

            static async Task SendStaticFullContact(Message message)
            {
                await botClient.SendContactAsync(
                    chatId: message.Chat.Id,
                    phoneNumber: "+1234567890",
                    firstName: "Han",
                    vCard: "BEGIN:VCARD\n" +
                            "VERSION:3.0\n" +
                            "N:Solo;Han\n" +
                            "ORG:Scruffy-looking nerf herder\n" +
                            "TEL;TYPE=voice,work,pref:+1234567890\n" +
                            "EMAIL:hansolo@mfalcon.com\n" +
                            "END:VCARD"
                    );
            }

            static async Task SendStaticSimpleContact(Message message)
            {
                await botClient.SendContactAsync(
                    chatId: message.Chat.Id,
                    phoneNumber: "+1234567890",
                    firstName: "Han",
                    lastName: "Solo"
                    );
            }

            static async Task SendStaticFullLocation(Message message)
            {
                await botClient.SendVenueAsync(
                   chatId: message.Chat.Id,
                   latitude: 50.0840172f,
                   longitude: 14.418288f,
                   title: "Man Hanging out",
                   address: "Husova, 110 00 Staré Město, Czechia"
                   );
            }

            static async Task SendStaticLocationSimple(Message message)
            {
                await botClient.SendLocationAsync(
                 chatId: message.Chat.Id,
                 latitude: 50.0840172f,
                 longitude: 14.418288f
                 );
            }

            static async Task AboutBot(Message message)
            {
                var me = await botClient.GetMeAsync();

                await botClient.SendTextMessageAsync(
                   chatId: message.Chat.Id,
                   text: $"Meu username é: {me.Username} e meu nome é {me.FirstName} {me.LastName}"
               );
            }

            static async Task Usage(Message message)
            {
                const string usage = "Olá, segue nossas opções:\n" +
                                        "/inline   - mostra ooções na linha\n" +
                                        "/keyboard - mostra um teclado personalizado com opções\n" +
                                        "/photo    - envia uma foto\n" +
                                        "/request  - pede pra escolher entre enviar o contato ou a localização\n" +
                                        "/contactFull - envia um contato estático com card\n" +
                                        "/contactSimple - envia um contato estático simples\n" +
                                        "/locationFull - envia a localização completa\n" +
                                        "/locationSimple - envia a localização simples\n" +
                                        "/about - sobre o bot";
                await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: usage,
                    replyMarkup: new ReplyKeyboardRemove()
                );
            }

        }

        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received answer {callbackQuery.Data}"
            );

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received send {callbackQuery.Data}"
            );
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }
    }
}
