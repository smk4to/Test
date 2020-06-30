using System.Collections.Generic;

namespace Asd_LocalServer.Models
{
    public class Asd_Response
    {
        public string CorrelationId { get; }
        public object Data { get; }
        public int Code { get; }
        public string Message { get; }

        public Asd_Response(Asd_E_ResponseCode code, string message, object data, string correlationId)
        {
            CorrelationId = correlationId;
            Data = data;
            Code = (int)code;
            Message = code switch
            {
                Asd_E_ResponseCode.Success => string.Empty,

                Asd_E_ResponseCode.ModelStateError => "Ошибка при валидации запроса.",
                Asd_E_ResponseCode.ValidationError => "Ошибка при валидации данных запроса.",
                Asd_E_ResponseCode.IncorrectRequest => "Не корректный запрос.",

                Asd_E_ResponseCode.GetException => "Критическая ошибка при получении данных.",
                Asd_E_ResponseCode.PostException => "Критическая ошибка при добавлении данных.",
                Asd_E_ResponseCode.PutException => "Критическая ошибка при обновлении данных.",
                Asd_E_ResponseCode.DeleteException => "Критическая ошибка при удалении данных",

                Asd_E_ResponseCode.GetError => "Ошибка при получении данных.",
                Asd_E_ResponseCode.PostError => "Ошибка при добавлении данных.",
                Asd_E_ResponseCode.PutError => "Ошибка при обновлении данных.",
                Asd_E_ResponseCode.DeleteError => "Ошибка при удалении данных",

                Asd_E_ResponseCode.UnknownError => "Неизвестная ошибка при обработке запроса.",
                _ => "Неизвестная ошибка при обработки запроса."
            };
            if (!string.IsNullOrEmpty(message)) Message = string.Join(" ", new List<string> { Message, message });
        }
    }
    public enum Asd_E_ResponseCode
    {
        // успешный запрос
        Success = 0,

        // ошибка при валидации запроса
        ModelStateError = 100,
        // ошибка при валидации запроса
        ValidationError = 101,
        // не корректный запрос
        IncorrectRequest = 102,

        // исключение при получении данных
        GetException = 110,
        // исключение при добавлении данных
        PostException = 111,
        // исключение при обновлении данных
        PutException = 112,
        // исключение при удалении данных
        DeleteException = 113,

        // ошибка при получении данных
        GetError = 115,
        // ошибка при добавлении данных
        PostError = 116,
        // ошибка при обновлении данных
        PutError = 117,
        // ошибка при удалении данных
        DeleteError = 118,

        // неизвестная ошибка
        UnknownError = 900
    }
}
