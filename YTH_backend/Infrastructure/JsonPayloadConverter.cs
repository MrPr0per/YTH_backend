using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YTH_backend.Models.ExpertApplication;

namespace YTH_backend.Infrastructure;


[JsonSerializable(typeof(AcceptedPayload))]
[JsonSerializable(typeof(ReviewedPayload))]
internal partial class JsonPayloadConverter : JsonSerializerContext
{
    
}