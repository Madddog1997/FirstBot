// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using FirstBot.Middlewares;
using FirstBot.Dialogs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FirstBot.Middleware;

namespace FirstBot
{
    public class Startup
    {
        private const string BotOpenIdMetadataKey = "BotOpenIdMetadata";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Create the Bot Framework Adapter.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state.
            services.AddSingleton<UserState>();

            // Create the Conversation state.
            services.AddSingleton<ConversationState>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.

            services.AddSingleton<RootDialog>(s => new RootDialog(new LuisRecognizer(new LuisApplication("0143846c-cd6d-45b5-87cf-2f0148a9bb48", "6ac14ad4850b4d598a92227b3cc77748", "https://westus.api.cognitive.microsoft.com"))));
            services.AddBot<EchoBot<RootDialog>>(opt =>
            {
                //opt.Middleware.Add(new CancelAndHelp());
                opt.Middleware.Add(new TranscriptLoggerMiddleware(new Logger()));
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseBotFramework();
            app.UseMvc();
        }
    }
}
