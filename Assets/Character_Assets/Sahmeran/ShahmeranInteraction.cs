using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShahmeranInteraction : MonoBehaviour
{
    public GameObject dialoguePanel; // Sohbet paneli
    public TextMeshProUGUI dialogueText; // Sohbet geçmiþini gösteren metin alaný
    public TMP_InputField playerInputField; // Oyuncunun mesajýný yazdýðý Input Field
    public OpenAIIntegration aiIntegration; // Yapay zeka entegrasyonu

    private string currentRiddle; // Þu anki bilmece
    private string correctAnswer; // Doðru cevap
    private bool abilityUnlocked = false; // Yetenek kazanýldý mý?
    private bool riddleAsked = false; // Bilmece soruldu mu?
    private int incorrectAttempts = 0; // Yanlýþ cevap sayýsý
    private const int maxAttempts = 3; // Maksimum deneme hakký

    private List<string> chatHistory = new List<string>(); // Sohbet geçmiþini tutar

    private enum ConversationState { Introduction, Riddle, Reward }
    private ConversationState currentState = ConversationState.Introduction;

    void Start()
    {
        aiIntegration = FindObjectOfType<OpenAIIntegration>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartConversation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CloseDialogue();
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SendPlayerMessage()
    {
        string playerMessage = playerInputField.text.Trim();
        if (!string.IsNullOrEmpty(playerMessage))
        {
            playerInputField.text = "";
            AddToChatHistory("Ülgen Han: " + playerMessage); // Oyuncunun mesajýný geçmiþe ekle
            ProcessPlayerMessage(playerMessage);
        }
    }

    private void StartConversation()
    {
        string introductionMessage = "Hoþ geldin, Ülgen Han. Ben Þahmeran, Türk mitolojisinin bilgeliðini temsil ediyorum. Sana Erlik'i yenmende yardýmcý olacaðým.";
        AddToChatHistory("Þahmeran: " + introductionMessage);
        currentState = ConversationState.Introduction;
    }

    private void ProcessPlayerMessage(string playerMessage)
    {
        switch (currentState)
        {
            case ConversationState.Introduction:
                HandleIntroduction(playerMessage);
                break;
            case ConversationState.Riddle:
                HandleRiddle(playerMessage);
                break;
            case ConversationState.Reward:
                AddToChatHistory("Þahmeran: Yolun açýk olsun, Ülgen Han.");
                break;
        }
    }

    private void HandleIntroduction(string playerMessage)
    {
        if (playerMessage.ToLower().Contains("erlik"))
        {
            string message = "Erlik'i yenmek için bilgeliðimi test etmelisin. Sana bir bilmece soracaðým.";
            AddToChatHistory("Þahmeran: " + message);
            AskRiddle();
            currentState = ConversationState.Riddle;
        }
        else
        {
            string message = "Üzgünüm, bu konuda yardýmcý olamam. Bana Erlik'i yenmekle ilgili sorular sorabilirsin.";
            AddToChatHistory("Þahmeran: " + message);
        }
    }

    private void AskRiddle()
    {
        if (!riddleAsked)
        {
            aiIntegration.StartCoroutine(aiIntegration.SendMessageToAI("Türk mitolojisiyle ilgili bir bilmece sor.", OnRiddleReceived));
            riddleAsked = true;
        }
    }

    private void OnRiddleReceived(string aiMessage)
    {
        string message;
        if (aiMessage.ToLower().Contains("üzgünüm") || aiMessage.ToLower().Contains("bilmiyorum"))
        {
            message = "Bilmeceyi sormakta bir sorun yaþadým. Sana Türk mitolojisiyle ilgili baþka bir bilmece sunacaðým.";
            AddToChatHistory("Þahmeran: " + message);
            AskRiddle();
        }
        else
        {
            message = "Bilmecem þu: " + aiMessage;
            AddToChatHistory("Þahmeran: " + message);
            currentRiddle = aiMessage;
            correctAnswer = "yýlan"; // Doðru cevabý burada ayarlayýn
        }
    }

    private void HandleRiddle(string playerAnswer)
    {
        if (playerAnswer.ToLower() == correctAnswer.ToLower())
        {
            string message = "Doðru cevap, Ülgen Han! Artýk özel yeteneðini kullanabilirsin.";
            AddToChatHistory("Þahmeran: " + message);
            abilityUnlocked = true;
            currentState = ConversationState.Reward;
        }
        else
        {
            incorrectAttempts++;
            if (incorrectAttempts >= maxAttempts)
            {
                string message = $"Üzgünüm, çok fazla yanlýþ cevap verdiniz. Doðru cevap: {correctAnswer}";
                AddToChatHistory("Þahmeran: " + message);
                currentState = ConversationState.Reward;
            }
            else
            {
                string message = "Yanlýþ cevap. Ýpucu: Türk mitolojisinde bilgeliði temsil eden bir sembol düþün.";
                AddToChatHistory("Þahmeran: " + message);
            }
        }
    }

    private void AddToChatHistory(string message)
    {
        chatHistory.Add(message);
        dialogueText.text = string.Join("\n", chatHistory); // Sohbet geçmiþini güncelle
    }
}
