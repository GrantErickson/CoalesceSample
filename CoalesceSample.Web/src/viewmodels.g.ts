import * as $metadata from './metadata.g'
import * as $models from './models.g'
import * as $apiClients from './api-clients.g'
import { ViewModel, ListViewModel, ServiceViewModel, DeepPartial, defineProps } from 'coalesce-vue/lib/viewmodel'

export interface GameViewModel extends $models.Game {
  gameId: string | null;
  name: string | null;
  description: string | null;
  releaseDate: Date | null;
  likes: number | null;
  totalRating: number | null;
  numberOfRatings: number | null;
  averageRating: number | null;
  averageDurationInHours: number | null;
  maxPlayers: number | null;
  minPlayers: number | null;
  genreId: number | null;
  genre: GenreViewModel | null;
  imageId: number | null;
  image: ImageViewModel | null;
  gameTags: GameTagViewModel[] | null;
  reviews: ReviewViewModel[] | null;
}
export class GameViewModel extends ViewModel<$models.Game, $apiClients.GameApiClient, string> implements $models.Game  {
  
  
  public addToGameTags() {
    return this.$addChild('gameTags') as GameTagViewModel
  }
  
  get tag(): ReadonlyArray<TagViewModel> {
    return (this.gameTags || []).map($ => $.tag!).filter($ => $)
  }
  
  constructor(initialData?: DeepPartial<$models.Game> | null) {
    super($metadata.Game, new $apiClients.GameApiClient(), initialData)
  }
}
defineProps(GameViewModel, $metadata.Game)

export class GameListViewModel extends ListViewModel<$models.Game, $apiClients.GameApiClient, GameViewModel> {
  
  constructor() {
    super($metadata.Game, new $apiClients.GameApiClient())
  }
}


export interface GameTagViewModel extends $models.GameTag {
  gameTagId: number | null;
  tagId: number | null;
  tag: TagViewModel | null;
  gameId: string | null;
  game: GameViewModel | null;
}
export class GameTagViewModel extends ViewModel<$models.GameTag, $apiClients.GameTagApiClient, number> implements $models.GameTag  {
  
  constructor(initialData?: DeepPartial<$models.GameTag> | null) {
    super($metadata.GameTag, new $apiClients.GameTagApiClient(), initialData)
  }
}
defineProps(GameTagViewModel, $metadata.GameTag)

export class GameTagListViewModel extends ListViewModel<$models.GameTag, $apiClients.GameTagApiClient, GameTagViewModel> {
  
  constructor() {
    super($metadata.GameTag, new $apiClients.GameTagApiClient())
  }
}


export interface GenreViewModel extends $models.Genre {
  genreId: number | null;
  name: string | null;
  description: string | null;
  games: GameViewModel[] | null;
}
export class GenreViewModel extends ViewModel<$models.Genre, $apiClients.GenreApiClient, number> implements $models.Genre  {
  
  
  public addToGames() {
    return this.$addChild('games') as GameViewModel
  }
  
  constructor(initialData?: DeepPartial<$models.Genre> | null) {
    super($metadata.Genre, new $apiClients.GenreApiClient(), initialData)
  }
}
defineProps(GenreViewModel, $metadata.Genre)

export class GenreListViewModel extends ListViewModel<$models.Genre, $apiClients.GenreApiClient, GenreViewModel> {
  
  constructor() {
    super($metadata.Genre, new $apiClients.GenreApiClient())
  }
}


export interface ImageViewModel extends $models.Image {
  imageId: number | null;
  base64Image: string | null;
}
export class ImageViewModel extends ViewModel<$models.Image, $apiClients.ImageApiClient, number> implements $models.Image  {
  
  constructor(initialData?: DeepPartial<$models.Image> | null) {
    super($metadata.Image, new $apiClients.ImageApiClient(), initialData)
  }
}
defineProps(ImageViewModel, $metadata.Image)

export class ImageListViewModel extends ListViewModel<$models.Image, $apiClients.ImageApiClient, ImageViewModel> {
  
  constructor() {
    super($metadata.Image, new $apiClients.ImageApiClient())
  }
}


export interface ReviewViewModel extends $models.Review {
  reviewId: string | null;
  rating: number | null;
  reviewDate: Date | null;
  reviewerName: string | null;
  reviewTitle: string | null;
  reviewBody: string | null;
  isDeleted: boolean | null;
  gameId: number | null;
  reviewedGame: GameViewModel | null;
}
export class ReviewViewModel extends ViewModel<$models.Review, $apiClients.ReviewApiClient, string> implements $models.Review  {
  
  constructor(initialData?: DeepPartial<$models.Review> | null) {
    super($metadata.Review, new $apiClients.ReviewApiClient(), initialData)
  }
}
defineProps(ReviewViewModel, $metadata.Review)

export class ReviewListViewModel extends ListViewModel<$models.Review, $apiClients.ReviewApiClient, ReviewViewModel> {
  
  constructor() {
    super($metadata.Review, new $apiClients.ReviewApiClient())
  }
}


export interface TagViewModel extends $models.Tag {
  tagId: number | null;
  name: string | null;
  description: string | null;
  gameTags: GameTagViewModel[] | null;
}
export class TagViewModel extends ViewModel<$models.Tag, $apiClients.TagApiClient, number> implements $models.Tag  {
  
  
  public addToGameTags() {
    return this.$addChild('gameTags') as GameTagViewModel
  }
  
  get game(): ReadonlyArray<GameViewModel> {
    return (this.gameTags || []).map($ => $.game!).filter($ => $)
  }
  
  constructor(initialData?: DeepPartial<$models.Tag> | null) {
    super($metadata.Tag, new $apiClients.TagApiClient(), initialData)
  }
}
defineProps(TagViewModel, $metadata.Tag)

export class TagListViewModel extends ListViewModel<$models.Tag, $apiClients.TagApiClient, TagViewModel> {
  
  constructor() {
    super($metadata.Tag, new $apiClients.TagApiClient())
  }
}


export class ApplicationUserServiceViewModel extends ServiceViewModel<typeof $metadata.ApplicationUserService, $apiClients.ApplicationUserServiceApiClient> {
  
  public get getRoles() {
    const getRoles = this.$apiClient.$makeCaller(
      this.$metadata.methods.getRoles,
      (c) => c.getRoles(),
      () => ({}),
      (c, args) => c.getRoles())
    
    Object.defineProperty(this, 'getRoles', {value: getRoles});
    return getRoles
  }
  
  public get hasRole() {
    const hasRole = this.$apiClient.$makeCaller(
      this.$metadata.methods.hasRole,
      (c, role: string | null) => c.hasRole(role),
      () => ({role: null as string | null, }),
      (c, args) => c.hasRole(args.role))
    
    Object.defineProperty(this, 'hasRole', {value: hasRole});
    return hasRole
  }
  
  public get getUserReviews() {
    const getUserReviews = this.$apiClient.$makeCaller(
      this.$metadata.methods.getUserReviews,
      (c) => c.getUserReviews(),
      () => ({}),
      (c, args) => c.getUserReviews())
    
    Object.defineProperty(this, 'getUserReviews', {value: getUserReviews});
    return getUserReviews
  }
  
  constructor() {
    super($metadata.ApplicationUserService, new $apiClients.ApplicationUserServiceApiClient())
  }
}


export class GameServiceViewModel extends ServiceViewModel<typeof $metadata.GameService, $apiClients.GameServiceApiClient> {
  
  public get getGames() {
    const getGames = this.$apiClient.$makeCaller(
      this.$metadata.methods.getGames,
      (c) => c.getGames(),
      () => ({}),
      (c, args) => c.getGames())
    
    Object.defineProperty(this, 'getGames', {value: getGames});
    return getGames
  }
  
  public get getGamesFromIds() {
    const getGamesFromIds = this.$apiClient.$makeCaller(
      this.$metadata.methods.getGamesFromIds,
      (c, gameIds: string[] | null) => c.getGamesFromIds(gameIds),
      () => ({gameIds: null as string[] | null, }),
      (c, args) => c.getGamesFromIds(args.gameIds))
    
    Object.defineProperty(this, 'getGamesFromIds', {value: getGamesFromIds});
    return getGamesFromIds
  }
  
  public get getGameDetails() {
    const getGameDetails = this.$apiClient.$makeCaller(
      this.$metadata.methods.getGameDetails,
      (c, gameId: string | null) => c.getGameDetails(gameId),
      () => ({gameId: null as string | null, }),
      (c, args) => c.getGameDetails(args.gameId))
    
    Object.defineProperty(this, 'getGameDetails', {value: getGameDetails});
    return getGameDetails
  }
  
  public get getGameImage() {
    const getGameImage = this.$apiClient.$makeCaller(
      this.$metadata.methods.getGameImage,
      (c, gameId: string | null) => c.getGameImage(gameId),
      () => ({gameId: null as string | null, }),
      (c, args) => c.getGameImage(args.gameId))
    
    Object.defineProperty(this, 'getGameImage', {value: getGameImage});
    return getGameImage
  }
  
  public get uploadGameImage() {
    const uploadGameImage = this.$apiClient.$makeCaller(
      this.$metadata.methods.uploadGameImage,
      (c, gameId: string | null, image: File | null) => c.uploadGameImage(gameId, image),
      () => ({gameId: null as string | null, image: null as File | null, }),
      (c, args) => c.uploadGameImage(args.gameId, args.image))
    
    Object.defineProperty(this, 'uploadGameImage', {value: uploadGameImage});
    return uploadGameImage
  }
  
  public get getAllTags() {
    const getAllTags = this.$apiClient.$makeCaller(
      this.$metadata.methods.getAllTags,
      (c) => c.getAllTags(),
      () => ({}),
      (c, args) => c.getAllTags())
    
    Object.defineProperty(this, 'getAllTags', {value: getAllTags});
    return getAllTags
  }
  
  public get getGameTags() {
    const getGameTags = this.$apiClient.$makeCaller(
      this.$metadata.methods.getGameTags,
      (c, gameId: string | null) => c.getGameTags(gameId),
      () => ({gameId: null as string | null, }),
      (c, args) => c.getGameTags(args.gameId))
    
    Object.defineProperty(this, 'getGameTags', {value: getGameTags});
    return getGameTags
  }
  
  public get setGameTags() {
    const setGameTags = this.$apiClient.$makeCaller(
      this.$metadata.methods.setGameTags,
      (c, gameId: string | null, tagIds: number[] | null) => c.setGameTags(gameId, tagIds),
      () => ({gameId: null as string | null, tagIds: null as number[] | null, }),
      (c, args) => c.setGameTags(args.gameId, args.tagIds))
    
    Object.defineProperty(this, 'setGameTags', {value: setGameTags});
    return setGameTags
  }
  
  public get addLike() {
    const addLike = this.$apiClient.$makeCaller(
      this.$metadata.methods.addLike,
      (c, gameId: string | null) => c.addLike(gameId),
      () => ({gameId: null as string | null, }),
      (c, args) => c.addLike(args.gameId))
    
    Object.defineProperty(this, 'addLike', {value: addLike});
    return addLike
  }
  
  public get removeLike() {
    const removeLike = this.$apiClient.$makeCaller(
      this.$metadata.methods.removeLike,
      (c, gameId: string | null) => c.removeLike(gameId),
      () => ({gameId: null as string | null, }),
      (c, args) => c.removeLike(args.gameId))
    
    Object.defineProperty(this, 'removeLike', {value: removeLike});
    return removeLike
  }
  
  constructor() {
    super($metadata.GameService, new $apiClients.GameServiceApiClient())
  }
}


export class LoginServiceViewModel extends ServiceViewModel<typeof $metadata.LoginService, $apiClients.LoginServiceApiClient> {
  
  public get login() {
    const login = this.$apiClient.$makeCaller(
      this.$metadata.methods.login,
      (c, email: string | null, password: string | null) => c.login(email, password),
      () => ({email: null as string | null, password: null as string | null, }),
      (c, args) => c.login(args.email, args.password))
    
    Object.defineProperty(this, 'login', {value: login});
    return login
  }
  
  public get getToken() {
    const getToken = this.$apiClient.$makeCaller(
      this.$metadata.methods.getToken,
      (c, email: string | null, password: string | null) => c.getToken(email, password),
      () => ({email: null as string | null, password: null as string | null, }),
      (c, args) => c.getToken(args.email, args.password))
    
    Object.defineProperty(this, 'getToken', {value: getToken});
    return getToken
  }
  
  public get logout() {
    const logout = this.$apiClient.$makeCaller(
      this.$metadata.methods.logout,
      (c) => c.logout(),
      () => ({}),
      (c, args) => c.logout())
    
    Object.defineProperty(this, 'logout', {value: logout});
    return logout
  }
  
  public get createAccount() {
    const createAccount = this.$apiClient.$makeCaller(
      this.$metadata.methods.createAccount,
      (c, name: string | null, email: string | null, password: string | null) => c.createAccount(name, email, password),
      () => ({name: null as string | null, email: null as string | null, password: null as string | null, }),
      (c, args) => c.createAccount(args.name, args.email, args.password))
    
    Object.defineProperty(this, 'createAccount', {value: createAccount});
    return createAccount
  }
  
  public get changePassword() {
    const changePassword = this.$apiClient.$makeCaller(
      this.$metadata.methods.changePassword,
      (c, currentPassword: string | null, newPassword: string | null) => c.changePassword(currentPassword, newPassword),
      () => ({currentPassword: null as string | null, newPassword: null as string | null, }),
      (c, args) => c.changePassword(args.currentPassword, args.newPassword))
    
    Object.defineProperty(this, 'changePassword', {value: changePassword});
    return changePassword
  }
  
  public get isLoggedIn() {
    const isLoggedIn = this.$apiClient.$makeCaller(
      this.$metadata.methods.isLoggedIn,
      (c) => c.isLoggedIn(),
      () => ({}),
      (c, args) => c.isLoggedIn())
    
    Object.defineProperty(this, 'isLoggedIn', {value: isLoggedIn});
    return isLoggedIn
  }
  
  public get getUserInfo() {
    const getUserInfo = this.$apiClient.$makeCaller(
      this.$metadata.methods.getUserInfo,
      (c) => c.getUserInfo(),
      () => ({}),
      (c, args) => c.getUserInfo())
    
    Object.defineProperty(this, 'getUserInfo', {value: getUserInfo});
    return getUserInfo
  }
  
  constructor() {
    super($metadata.LoginService, new $apiClients.LoginServiceApiClient())
  }
}


export class ReviewServiceViewModel extends ServiceViewModel<typeof $metadata.ReviewService, $apiClients.ReviewServiceApiClient> {
  
  public get getReviews() {
    const getReviews = this.$apiClient.$makeCaller(
      this.$metadata.methods.getReviews,
      (c, gameId: string | null) => c.getReviews(gameId),
      () => ({gameId: null as string | null, }),
      (c, args) => c.getReviews(args.gameId))
    
    Object.defineProperty(this, 'getReviews', {value: getReviews});
    return getReviews
  }
  
  public get addReview() {
    const addReview = this.$apiClient.$makeCaller(
      this.$metadata.methods.addReview,
      (c, gameId: string | null, reviewTitle: string | null, reviewBody: string | null, rating: number | null) => c.addReview(gameId, reviewTitle, reviewBody, rating),
      () => ({gameId: null as string | null, reviewTitle: null as string | null, reviewBody: null as string | null, rating: null as number | null, }),
      (c, args) => c.addReview(args.gameId, args.reviewTitle, args.reviewBody, args.rating))
    
    Object.defineProperty(this, 'addReview', {value: addReview});
    return addReview
  }
  
  public get deleteReview() {
    const deleteReview = this.$apiClient.$makeCaller(
      this.$metadata.methods.deleteReview,
      (c, reviewId: string | null) => c.deleteReview(reviewId),
      () => ({reviewId: null as string | null, }),
      (c, args) => c.deleteReview(args.reviewId))
    
    Object.defineProperty(this, 'deleteReview', {value: deleteReview});
    return deleteReview
  }
  
  constructor() {
    super($metadata.ReviewService, new $apiClients.ReviewServiceApiClient())
  }
}


const viewModelTypeLookup = ViewModel.typeLookup = {
  Game: GameViewModel,
  GameTag: GameTagViewModel,
  Genre: GenreViewModel,
  Image: ImageViewModel,
  Review: ReviewViewModel,
  Tag: TagViewModel,
}
const listViewModelTypeLookup = ListViewModel.typeLookup = {
  Game: GameListViewModel,
  GameTag: GameTagListViewModel,
  Genre: GenreListViewModel,
  Image: ImageListViewModel,
  Review: ReviewListViewModel,
  Tag: TagListViewModel,
}
const serviceViewModelTypeLookup = ServiceViewModel.typeLookup = {
  ApplicationUserService: ApplicationUserServiceViewModel,
  GameService: GameServiceViewModel,
  LoginService: LoginServiceViewModel,
  ReviewService: ReviewServiceViewModel,
}

