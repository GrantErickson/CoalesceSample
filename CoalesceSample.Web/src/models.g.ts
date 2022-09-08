import * as metadata from './metadata.g'
import { Model, DataSource, convertToModel, mapToModel } from 'coalesce-vue/lib/model'

export interface Game extends Model<typeof metadata.Game> {
  gameId: string | null
  name: string | null
  description: string | null
  releaseDate: Date | null
  likes: number | null
  totalRating: number | null
  numberOfRatings: number | null
  averageRating: number | null
  averageDurationInHours: number | null
  maxPlayers: number | null
  minPlayers: number | null
  genreId: number | null
  genre: Genre | null
  imageId: number | null
  image: Image | null
  gameTags: GameTag[] | null
  reviews: Review[] | null
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
export namespace Game {
  export namespace DataSources {
    
    export class GameDataSource implements DataSource<typeof metadata.Game.dataSources.gameDataSource> {
      readonly $metadata = metadata.Game.dataSources.gameDataSource
      filterTags: string | null = null
      filterRatingsUpper: number | null = null
      filterRatingsLower: number | null = null
    }
  }
}


export interface GameTag extends Model<typeof metadata.GameTag> {
  gameTagId: number | null
  tagId: number | null
  tag: Tag | null
  gameId: string | null
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


export interface Image extends Model<typeof metadata.Image> {
  imageId: number | null
  base64Image: string | null
}
export class Image {
  
  /** Mutates the input object and its descendents into a valid Image implementation. */
  static convert(data?: Partial<Image>): Image {
    return convertToModel(data || {}, metadata.Image) 
  }
  
  /** Maps the input object and its descendents to a new, valid Image implementation. */
  static map(data?: Partial<Image>): Image {
    return mapToModel(data || {}, metadata.Image) 
  }
  
  /** Instantiate a new Image, optionally basing it on the given data. */
  constructor(data?: Partial<Image> | {[k: string]: any}) {
      Object.assign(this, Image.map(data || {}));
  }
}


export interface Review extends Model<typeof metadata.Review> {
  reviewId: string | null
  rating: number | null
  reviewDate: Date | null
  reviewerName: string | null
  reviewTitle: string | null
  reviewBody: string | null
  isDeleted: boolean | null
  gameId: string | null
  reviewedGame: Game | null
}
export class Review {
  
  /** Mutates the input object and its descendents into a valid Review implementation. */
  static convert(data?: Partial<Review>): Review {
    return convertToModel(data || {}, metadata.Review) 
  }
  
  /** Maps the input object and its descendents to a new, valid Review implementation. */
  static map(data?: Partial<Review>): Review {
    return mapToModel(data || {}, metadata.Review) 
  }
  
  /** Instantiate a new Review, optionally basing it on the given data. */
  constructor(data?: Partial<Review> | {[k: string]: any}) {
      Object.assign(this, Review.map(data || {}));
  }
}


export interface Tag extends Model<typeof metadata.Tag> {
  tagId: number | null
  name: string | null
  description: string | null
  gameTags: GameTag[] | null
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


export interface UserInfoDto extends Model<typeof metadata.UserInfoDto> {
  name: string | null
  email: string | null
  userRoles: string[] | null
}
export class UserInfoDto {
  
  /** Mutates the input object and its descendents into a valid UserInfoDto implementation. */
  static convert(data?: Partial<UserInfoDto>): UserInfoDto {
    return convertToModel(data || {}, metadata.UserInfoDto) 
  }
  
  /** Maps the input object and its descendents to a new, valid UserInfoDto implementation. */
  static map(data?: Partial<UserInfoDto>): UserInfoDto {
    return mapToModel(data || {}, metadata.UserInfoDto) 
  }
  
  /** Instantiate a new UserInfoDto, optionally basing it on the given data. */
  constructor(data?: Partial<UserInfoDto> | {[k: string]: any}) {
      Object.assign(this, UserInfoDto.map(data || {}));
  }
}


