using Xunit;
using Moq;
using Moq.Protected;
using System.Net;
using GoogleTimezoneSln.Services;
using GoogleTimezoneSln.Models;
using System.Text.Json;

public class TimezoneTests
{
    [Fact]
    public async Task GetTimeZoneAsync_ReturnsCorrectData()
    {
        // Arrange
        var request = new TimeZoneRequest
        {
            Latitude = 48.3794,
            Longitude = 31.1656,
            Timestamp = 1749077406
        };

        var expectedResponse = new TimeZoneResponse
        {
            TimeZoneId = "Europe/Kiev",
            TimeZoneName = "Eastern European Summer Time",
            RawOffset = 7200,
            DstOffset = 3600,
            Status = "OK"
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse);

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>("SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>())
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent(jsonResponse),
           });

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new TimeZoneService(httpClient, "fake-api-key");

        // Act
        var actual = await service.GetTimeZoneAsync(request);

        // Assert
        Assert.Equal(expectedResponse.TimeZoneId, actual.TimeZoneId);
        Assert.Equal(expectedResponse.TimeZoneName, actual.TimeZoneName);
        Assert.Equal(expectedResponse.RawOffset, actual.RawOffset);
        Assert.Equal(expectedResponse.DstOffset, actual.DstOffset);
    }

    [Fact]
    public async Task GetTimeZoneAsync_ThrowsOnInvalidJson()
    {
        // Arrange
        var request = new TimeZoneRequest { Latitude = 0, Longitude = 0, Timestamp = 0 };

        var invalidJson = "{ not-a-valid-json";

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidJson),
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new TimeZoneService(httpClient, "fake-api-key");

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() => service.GetTimeZoneAsync(request));
    }

    [Fact]
    public async Task GetTimeZoneAsync_HandlesHttpError()
    {
        // Arrange
        var request = new TimeZoneRequest { Latitude = 0, Longitude = 0, Timestamp = 0 };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Not Found")
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var service = new TimeZoneService(httpClient, "fake-api-key");

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GetTimeZoneAsync(request));
    }

}

//написи врапт клас для обробки класів глянукти