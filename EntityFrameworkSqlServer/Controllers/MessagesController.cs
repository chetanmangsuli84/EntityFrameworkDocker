using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EntityFrameworkSqlServer.DataAccessLayer;
using EntityFrameworkSqlServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkSqlServer.Controllers
{

    public class Result
    {
        public Message message { get; set; }
        public string isPalindrome { get; set; }
    }
    /// <summary>Class MessagesController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase"/></summary>
    [ApiController, Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        /// <summary>The Messages database context</summary>
        private readonly EntityFrameworkSqlServerContext _MessagesDbContext;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(EntityFrameworkSqlServerContext MessagesDbContext, ILogger<MessagesController> logger)
        {
            _MessagesDbContext = MessagesDbContext ?? throw new ArgumentNullException(nameof(MessagesDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Message>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            try
            {
                return Ok(await _MessagesDbContext.Messages.ToListAsync());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured ", e.StackTrace);
                return NotFound();
            }
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            try
            {
                var message = await _MessagesDbContext.Messages.FindAsync(id);

                if (message == null)
                {
                    _logger.LogError($"Message with id: {id}, hasn't been found in database.");
                    return NotFound();

                }
                string result = string.Empty;
                string content = message.Message_text;
                if (IsPalindrome(content))
                    result = "Palindrome";
                else
                    result = "Not Palindrome";
                Result obj = new Result();
                obj.message = message;
                obj.isPalindrome = result;
                return Ok(obj);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured ", e.StackTrace);
                return NotFound();
            }
        }

        private bool IsPalindrome(string string1)
        {
            string rev = string.Empty;
            char[] ch = string1.ToCharArray();
            Array.Reverse(ch);
            rev = new string(ch);
            bool b = string1.Equals(rev, StringComparison.OrdinalIgnoreCase);
            return b;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Message>> Create([FromBody] Message message)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _MessagesDbContext.Messages.AddAsync(message);
                await _MessagesDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured ", e.StackTrace);
                return NotFound();
            }
            return Ok(message);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Message), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Message>> Update(int id, [FromBody] Message messageFromJson)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var message = await _MessagesDbContext.Messages.FindAsync(id);

                if (message == null)
                {
                    _logger.LogError($"Message with id: {id}, hasn't been found in database.");
                    return NotFound();

                }

                message.Message_text = messageFromJson.Message_text;


                await _MessagesDbContext.SaveChangesAsync();
                return Ok(message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured ", e.StackTrace);
                return NotFound();
            }

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Message>> Delete(int id)
        {
            try
            {
                var messsage = await _MessagesDbContext.Messages.FindAsync(id);

                if (messsage == null)
                {
                    _logger.LogError($"Message with id: {id}, hasn't been found in database.");
                    return NotFound();
                }

                _MessagesDbContext.Remove(messsage);
                await _MessagesDbContext.SaveChangesAsync();

                return Ok(messsage);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception Occured ", e.StackTrace);
                return NotFound();
            }
        }



    }
}