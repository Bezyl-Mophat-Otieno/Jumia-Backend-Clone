using Microsoft.AspNetCore.SignalR;

namespace AuthMS.SignalR.Hubs
{
    public class RegistrationHub:Hub
    {
        // This public methods can be invokeed from the clients
        public async Task SendMessage(string user , string message)
        {

            await Clients.All.SendAsync("OnRegistration",user,message);
        }
        // Add connected client to a group 

        public async Task AddToGroup(string groupname)
        {

            await Groups.AddToGroupAsync(Context.ConnectionId, groupname);


        }

        // You can remove the connected client from group

        public async Task RemoveFromGroup(string groupname)
        {

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupname);

        }

        /*The OnConnectedAsync method in the SignalR Hub is invoked whenever a new client connects to the hub. 
         * By default, it doesn't have any specific logic for grouping clients, so it's*/

      /*  public override async Task OnConnectedAsync()
        {
            // Determine the group name based on your logic (e.g., user roles)
            string groupName = "AdminGroup"; // Replace with your dynamic logic

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await base.OnConnectedAsync();
        } */

    }
}
