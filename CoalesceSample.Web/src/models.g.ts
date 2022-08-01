import * as metadata from './metadata.g'
import { Model, DataSource, convertToModel, mapToModel } from 'coalesce-vue/lib/model'

export interface Game extends Model<typeof metadata.Game> {
  gameId: number | null
  name: string | null
  description: string | null
  averageDurationInHours: number | null
  maxPlayers: number | null
  minPlayers: number | null
  genreId: number | null
  genre: Genre | null
  gameTags: GameTag[] | null
}
export class Game {
  
  /** Mutates the input object and its descendents into a valid Game implementation. */
  static convert(data?: Partial<Game>): Game {
    return convertToModel(data || {}, metadata.Game) 
  }
  
  /** Maps the input object and its descendents to a new, valid Game implementation. */
  static map(data?: Partial<Game>): Game {
    return mapToModel(data || {}, metadata.Game) 
  }
  
  /** Instantiate a new Game, optionally basing it on the given data. */
  constructor(data?: Partial<Game> | {[k: string]: any}) {
      Object.assign(this, Game.map(data || {}));
  }
}


export interface GameTag extends Model<typeof metadata.GameTag> {
  gameTagId: number | null
  tagId: number | null
  tag: Tag | null
  gameId: number | null
  game: Game | null
}
export class GameTag {
  
  /** Mutates the input object and its descendents into a valid GameTag implementation. */
  static convert(data?: Partial<GameTag>): GameTag {
    return convertToModel(data || {}, metadata.GameTag) 
  }
  
  /** Maps the input object and its descendents to a new, valid GameTag implementation. */
  static map(data?: Partial<GameTag>): GameTag {
    return mapToModel(data || {}, metadata.GameTag) 
  }
  
  /** Instantiate a new GameTag, optionally basing it on the given data. */
  constructor(data?: Partial<GameTag> | {[k: string]: any}) {
      Object.assign(this, GameTag.map(data || {}));
  }
}


export interface Genre extends Model<typeof metadata.Genre> {
  genreId: number | null
  name: string | null
  description: string | null
  games: Game[] | null
}
export class Genre {
  
  /** Mutates the input object and its descendents into a valid Genre implementation. */
  static convert(data?: Partial<Genre>): Genre {
    return convertToModel(data || {}, metadata.Genre) 
  }
  
  /** Maps the input object and its descendents to a new, valid Genre implementation. */
  static map(data?: Partial<Genre>): Genre {
    return mapToModel(data || {}, metadata.Genre) 
  }
  
  /** Instantiate a new Genre, optionally basing it on the given data. */
  constructor(data?: Partial<Genre> | {[k: string]: any}) {
      Object.assign(this, Genre.map(data || {}));
  }
}


export interface Tag extends Model<typeof metadata.Tag> {
  tagId: number | null
  name: string | null
  description: string | null
  games: GameTag[] | null
}
export class Tag {
  
  /** Mutates the input object and its descendents into a valid Tag implementation. */
  static convert(data?: Partial<Tag>): Tag {
    return convertToModel(data || {}, metadata.Tag) 
  }
  
  /** Maps the input object and its descendents to a new, valid Tag implementation. */
  static map(data?: Partial<Tag>): Tag {
    return mapToModel(data || {}, metadata.Tag) 
  }
  
  /** Instantiate a new Tag, optionally basing it on the given data. */
  constructor(data?: Partial<Tag> | {[k: string]: any}) {
      Object.assign(this, Tag.map(data || {}));
  }
}


