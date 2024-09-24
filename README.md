# Documancer [![License](https://img.shields.io/badge/license-Apache%202.0-blue)](https://github.com/Contykpo/Documancer/blob/master/LICENSE)

Documancer is an early-stage application dedicated to logging, tracking and AI-powered narration of campaigns and stories.

***

## The Plan

An automated GURPS gamebook with AI Dungeon Master or Adventure Narrator. The app must store timelines, players, characters, character sheets (and everything else they might involve. Should consider implementing slightly custom skills), campaigns, parties, story / adventure components, non-playable-character (alongside behavior and personality), maps, places, scenes. The app's aim is to narrate the flow of events, places in the world, character actions and the scenery during those events. As well as being used as a logger to keep track of every session made during a campaign. This will be done by keeping an organized database in which everything listed above will be stored and kept organized by timelines inside campaigns.

The app will be built on Blazor Web App template in order to take advantage of the benefits of Blazor Hybrid. We handle backend (EFC, MySQL/MongoDB and GPT interactions) from the Server-Side and the UI, graphs rendering from the Client-Side with WebAssembly. The Blazor Hybrid mode is also convenient when considering the possibility of Mobile and Desktop ports in the future.