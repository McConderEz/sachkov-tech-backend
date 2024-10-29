﻿using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using CSharpFunctionalExtensions;
using System.ComponentModel.DataAnnotations;

namespace EmailNotificationService.API;

public class MailSender
{
    private readonly MailOptions _options;
    private readonly ILogger<MailSender> _logger;
    private readonly EmailValidator _validator;

    public MailSender(
        IOptions<MailOptions> options,
        ILogger<MailSender> logger,
        EmailValidator validator)
    {
        _options = options.Value;
        _logger = logger;
        _validator = validator;
    }

    public async Task<UnitResult<string>> Send(MailData mailData) 
    {
        var validationResult = _validator.Execute(mailData.To);
        if (validationResult.IsFailure)
            return validationResult.Error;

        mailData.To = validationResult.Value;

        var mail = new MimeMessage();

        mail.From.Add(new MailboxAddress(_options.FromDisplayName, _options.From));

        foreach (var address in mailData.To)
        {
            MailboxAddress.TryParse(address, out var mailAddress);
            mail.To.Add(mailAddress!);
        }
            
        var body = new BodyBuilder { HtmlBody = mailData.Body };

        mail.Body = body.ToMessageBody();
        mail.Subject = mailData.Subject;

        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Host, _options.Port);
        await client.AuthenticateAsync(_options.UserName, _options.Password);
        await client.SendAsync(mail);

        foreach (var address in mail.To)
            _logger.LogInformation("Email succesfully sended to {to}", address);

        return UnitResult.Success<string>();
    }
}