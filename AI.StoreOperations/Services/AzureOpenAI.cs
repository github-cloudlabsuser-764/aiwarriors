using AI.StoreOperations.Model;
using Newtonsoft.Json;
using System.Web.Mvc;
using static AI.StoreOperations.Model.OpenAIRequest;
using OpenAIRequest = AI.StoreOperations.Model.OpenAIRequest;

namespace AI.StoreOperations.Services
{
    public class AzureOpenAI
    {
        private static readonly string endpoint = "https://cgaiservice.openai.azure.com/openai/deployments/my-gpt35-model/chat/completions?api-version=2024-02-15-preview"; // Your Azure OpenAI endpoint
        private static readonly string apiKey = "867ec0de987c46be8aabb85104726996"; // Your Azure OpenAI API key
        private static readonly string deploymentName = "my-gpt35-model"; // Name of your deployment

        public static JsonResult GetShelfOptimizationRecommendations(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Add("api-key", apiKey);               

                OpenAIRequest req = new OpenAIRequest()
                {
                    messages = new List<Message>() { new Message()
                    {
                        content = "You are an AI assistant that helps store manager in arranging the product in shelfs based on the customer type, sales volumes, season etc.. you also help manager to generate reports on various parameters.",
                        role = "system"
                    },
                    new Message()
                    {
                        content = prompt,
                        role = "user"
                    }
                    },
                    max_tokens = 2000,
                    temperature = 1,
                    frequency_penalty = 0,
                    presence_penalty = 0,
                    top_p = 0.5,
                    stop = null
                };
                var content = new StringContent(JsonConvert.SerializeObject(req), null, "application/json");
                request.Content = content;
                var response = client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                return null;
                //return response.Result;

            }
        }

    }
}
