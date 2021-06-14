using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppServer.DataAccess.Entities
{
	[Table("PersonalMessage")]
	public class PersonalMessageEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		
		public Guid SenderId { get; set; }

		public Guid ReceiverId { get; set; }

		public string Text { get; set; }

		public DateTime SentTime { get; set; }


		public UserEntity Sender { get; set; }

		public UserEntity Receiver { get; set; }
	}
}