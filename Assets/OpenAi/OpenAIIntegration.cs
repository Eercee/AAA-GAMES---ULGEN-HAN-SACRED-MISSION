using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAIIntegration : MonoBehaviour
{
    private string apiKey = "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"; // OpenAI API anahtarýnýzý buraya yapýþtýrýn
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    private List<string> previousMessages = new List<string>();

    public IEnumerator SendMessageToAI(string playerMessage, System.Action<string> callback)
    {
        AddToMemory("user", playerMessage);

        string jsonData = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [" +
                          "{\"role\": \"system\", \"content\": \"You are Shahmeran, a mythical figure in Turkish mythology. Your role is to help Ülgen Han defeat Erlik by providing riddles and helpful guidance related to mythology. Never respond with open-ended or unrelated answers.\"}," +
                          GetPreviousMessages() + "," +
                          "{\"role\": \"user\", \"content\": \"" + playerMessage + "\"}]}";

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            string aiMessage = ParseAIResponse(response);
            AddToMemory("assistant", aiMessage);
            callback(aiMessage);
        }
        else
        {
            Debug.LogError("API Error: " + request.error);
        }
    }

    private string ParseAIResponse(string jsonResponse)
    {
        var json = JsonUtility.FromJson<OpenAIResponse>(jsonResponse);
        return json.choices[0].message.content;
    }

    private void AddToMemory(string role, string message)
    {
        previousMessages.Add("{\"role\": \"" + role + "\", \"content\": \"" + message + "\"}");
    }

    private string GetPreviousMessages()
    {
        return string.Join(",", previousMessages);
    }
}

[System.Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

[System.Serializable]
public class Message
{
    public string content;
}
