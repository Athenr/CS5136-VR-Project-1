using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class WeatherScript : MonoBehaviour
{
    public TextMeshPro weatherText;

    private const string USER_AGENT = "For University Project (ningxn@mail.uc.edu)";

    private string lat = "39.1031";
    private string lon = "-84.5120";

    void Start()
    {
        InvokeRepeating(nameof(GetWeather), 0f, 300f);
    }

    void GetWeather() => StartCoroutine(FetchNWSData());

    IEnumerator FetchNWSData()
    {
        string pointsUrl = $"https://api.weather.gov/points/{lat},{lon}";

        using (UnityWebRequest request = UnityWebRequest.Get(pointsUrl))
        {
            request.SetRequestHeader("User-Agent", USER_AGENT);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                string forecastUrl = ExtractUrl(json, "\"forecast\": \"");

                yield return StartCoroutine(FetchActualForecast(forecastUrl));
            }
        }
    }

    string GetWeatherEmoji(string forecast)
    {
        forecast = forecast.ToLower();

        if (forecast.Contains("thunderstorm")) return "⛈";
        if (forecast.Contains("snow")) return "❄️";
        if (forecast.Contains("rain") || forecast.Contains("showers")) return "🌧️";
        if (forecast.Contains("cloudy")) return "☁️";
        if (forecast.Contains("sunny") || forecast.Contains("clear")) return "☀️";
        return "🌡️";
    }

    IEnumerator FetchActualForecast(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("User-Agent", USER_AGENT);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;

                int temp = (int)ExtractFloat(json, "\"temperature\": ", ",");
                string detail = ExtractString(json, "\"shortForecast\": \"", "\"");
                if (detail.Contains("then"))
                {
                    detail = detail.Split(" then")[0];
                }
                string emoji = GetWeatherEmoji(detail);
                weatherText.text = "Weather\n";
                weatherText.text = weatherText.text + $"{emoji}\n{temp}°F {detail}";
            }
        }
    }

    string ExtractUrl(string json, string key)
    {
        int start = json.IndexOf(key) + key.Length;
        int end = json.IndexOf("\"", start);
        return json.Substring(start, end - start);
    }

    float ExtractFloat(string json, string key, string endChar)
    {
        int start = json.IndexOf(key) + key.Length;
        int end = json.IndexOf(endChar, start);
        return float.Parse(json.Substring(start, end - start));
    }

    string ExtractString(string json, string key, string endChar)
    {
        int start = json.IndexOf(key) + key.Length;
        int end = json.IndexOf(endChar, start);
        return json.Substring(start, end - start);
    }
}