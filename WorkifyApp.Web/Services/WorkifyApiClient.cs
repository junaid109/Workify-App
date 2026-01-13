using WorkifyApp.ApiService.Domain;

namespace WorkifyApp.Web.Services;

public class WorkifyApiClient(HttpClient httpClient)
{
    // Clients
    public async Task<List<Client>> GetClientsAsync() 
        => await httpClient.GetFromJsonAsync<List<Client>>("/api/clients") ?? [];

    public async Task CreateClientAsync(Client client)
        => await httpClient.PostAsJsonAsync("/api/clients", client);

    // Projects
    public async Task<List<Project>> GetProjectsAsync()
        => await httpClient.GetFromJsonAsync<List<Project>>("/api/projects") ?? [];

    public async Task CreateProjectAsync(Project project)
        => await httpClient.PostAsJsonAsync("/api/projects", project);
    
    // Invoices
    public async Task<List<Invoice>> GetInvoicesAsync()
        => await httpClient.GetFromJsonAsync<List<Invoice>>("/api/invoices") ?? [];

    public async Task CreateInvoiceAsync(Invoice invoice)
        => await httpClient.PostAsJsonAsync("/api/invoices", invoice);

    // AI
    public async Task<string> ChatAsync(string message) 
    {
        var response = await httpClient.PostAsJsonAsync("/api/ai/chat", new { Message = message });
        return await response.Content.ReadAsStringAsync(); // Typically returns the string directly or wrapped
    }
}
