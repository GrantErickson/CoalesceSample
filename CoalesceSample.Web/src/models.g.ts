import * as metadata from './metadata.g'
import { Model, DataSource, convertToModel, mapToModel } from 'coalesce-vue/lib/model'

export interface ApplicationUser extends Model<typeof metadata.ApplicationUser> {
  applicationUserId: number | null
  name: string | null
}
export class ApplicationUser {
  
  /** Mutates the input object and its descendents into a valid ApplicationUser implementation. */
  static convert(data?: Partial<ApplicationUser>): ApplicationUser {
    return convertToModel(data || {}, metadata.ApplicationUser) 
  }
  
  /** Maps the input object and its descendents to a new, valid ApplicationUser implementation. */
  static map(data?: Partial<ApplicationUser>): ApplicationUser {
    return mapToModel(data || {}, metadata.ApplicationUser) 
  }
  
  /** Instantiate a new ApplicationUser, optionally basing it on the given data. */
  constructor(data?: Partial<ApplicationUser> | {[k: string]: any}) {
      Object.assign(this, ApplicationUser.map(data || {}));
  }
}


export interface Game extends Model<typeof metadata.Game> {
  gameId: number | null
  name: string | null
  description: string | null
  averageDurationInHours: number | null
  maxPlayers: number | null
  minPlayers: number | null
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


