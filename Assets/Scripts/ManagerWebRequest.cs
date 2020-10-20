using System.IO;
using System.Net;
using System.Threading.Tasks;

//TODO: Error Handler
public class ManagerWebRequest
{
    public static async Task<byte[]> GetRawBinFromWebRequestAsync(string uri)
    {
        //Stopwatch stopwatch = Stopwatch.StartNew();

        WebRequest request = WebRequest.Create(uri);

        WebResponse response = await request.GetResponseAsync();

        byte[] result = default;

        using (Stream stream = response.GetResponseStream())
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                result = reader.ReadBytes((int)stream.Length);
            }
        }
        response.Close();

        //stopwatch.Stop();

        //Debug.LogError($"uri {uri} {Environment.NewLine} Time {stopwatch.ElapsedMilliseconds}");

        return result;
    }
}



