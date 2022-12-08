-- Script Date: 09-12-2022 03:56  - ErikEJ.SqlCeScripting version 3.5.2.94
-- Database information:
-- Database: D:\ASPNET Core\IMDB-Movie-API\IMDB-Movie-API\imdb.db
-- ServerVersion: 3.38.5.1
-- DatabaseSize: 36 KB
-- Created: 09-12-2022 00:58

-- User Table information:
-- Number of tables: 5
-- __EFMigrationsHistory: -1 row(s)
-- ActorMovies: -1 row(s)
-- Actors: -1 row(s)
-- Movies: -1 row(s)
-- Producers: -1 row(s)

SELECT 1;
PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE [Producers] (
  [ProducerId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [ProducerName] text NOT NULL
);
CREATE TABLE [Movies] (
  [MovieId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [Title] text NULL
, [DateOfRelease] text NOT NULL
, [Producer] bigint NOT NULL
);
CREATE TABLE [Actors] (
  [ActorId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [ActorName] text NOT NULL
, [Gender] text NOT NULL
, [Bio] text NULL
, [DOB] text NOT NULL
);
CREATE TABLE [ActorMovies] (
  [MappingId] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [actorId] bigint NOT NULL
, [movieId] bigint NOT NULL
, CONSTRAINT [FK_ActorMovies_0_0] FOREIGN KEY ([movieId]) REFERENCES [Movies] ([MovieId]) ON DELETE CASCADE ON UPDATE NO ACTION
);
CREATE TABLE [__EFMigrationsHistory] (
  [MigrationId] text NOT NULL
, [ProductVersion] text NOT NULL
, CONSTRAINT [sqlite_autoindex___EFMigrationsHistory_1] PRIMARY KEY ([MigrationId])
);
CREATE INDEX [ActorMovies_IX_ActorMovies_movieId] ON [ActorMovies] ([movieId] ASC);
CREATE TRIGGER [fki_ActorMovies_movieId_Movies_MovieId] BEFORE Insert ON [ActorMovies] FOR EACH ROW BEGIN SELECT RAISE(ROLLBACK, 'Insert on table ActorMovies violates foreign key constraint FK_ActorMovies_0_0') WHERE (SELECT MovieId FROM Movies WHERE  MovieId = NEW.movieId) IS NULL; END;
CREATE TRIGGER [fku_ActorMovies_movieId_Movies_MovieId] BEFORE Update ON [ActorMovies] FOR EACH ROW BEGIN SELECT RAISE(ROLLBACK, 'Update on table ActorMovies violates foreign key constraint FK_ActorMovies_0_0') WHERE (SELECT MovieId FROM Movies WHERE  MovieId = NEW.movieId) IS NULL; END;
COMMIT;

