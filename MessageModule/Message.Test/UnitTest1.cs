using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Message.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Message.Client.Models.CreateMessageRequest model = new Message.Client.Models.CreateMessageRequest();
            model.NewMessage = new Message.Client.Models.ClientMessageModel();

            model.NewMessage.MessageType = "Confirmation";
            model.NewMessage.ProgramTime = DateTime.Now;
            model.NewMessage.UserAction = "Sebastian";

            model.NewMessage.RelatedParameter = new System.Collections.Generic.List<Client.Models.ClientMessageParameter>();
            model.NewMessage.RelatedParameter.Add(new Client.Models.ClientMessageParameter() { Key = "TO", Value = "sebastian.martinez@gmail.com,sebastian.martinez@gmail.com,sebastian.martinez@gmail.com" });
            model.NewMessage.RelatedParameter.Add(new Client.Models.ClientMessageParameter() { Key = "Name", Value = "sebastian.martinez@gmail.com" });
            model.NewMessage.RelatedParameter.Add(new Client.Models.ClientMessageParameter() { Key = "LastName", Value = "sebastian.martinez@gmail.com" });
            model.NewMessage.RelatedParameter.Add(new Client.Models.ClientMessageParameter() { Key = "Link", Value = "sebastian.martinez@gmail.com" });

            Message.Client.Models.CreateMessageResponse response = Message.Client.Client.Instance.CreateMessage(model);
        }
    }
}
