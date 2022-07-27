import * as $metadata from './metadata.g'
import * as $models from './models.g'
import * as $apiClients from './api-clients.g'
import { ViewModel, ListViewModel, ServiceViewModel, DeepPartial, defineProps } from 'coalesce-vue/lib/viewmodel'

export interface ApplicationUserViewModel extends $models.ApplicationUser {
  applicationUserId: number | null;
  name: string | null;
}
export class ApplicationUserViewModel extends ViewModel<$models.ApplicationUser, $apiClients.ApplicationUserApiClient, number> implements $models.ApplicationUser  {
  
  constructor(initialData?: DeepPartial<$models.ApplicationUser> | null) {
    super($metadata.ApplicationUser, new $apiClients.ApplicationUserApiClient(), initialData)
  }
}
defineProps(ApplicationUserViewModel, $metadata.ApplicationUser)

export class ApplicationUserListViewModel extends ListViewModel<$models.ApplicationUser, $apiClients.ApplicationUserApiClient, ApplicationUserViewModel> {
  
  constructor() {
    super($metadata.ApplicationUser, new $apiClients.ApplicationUserApiClient())
  }
}


export interface GameViewModel extends $models.Game {
  gameId: number | null;
  name: string | null;
  description: string | null;
  averageDurationInHours: number | null;
  maxPlayers: number | null;
  minPlayers: number | null;
  genreId: number | null;
  gameTags: $models.Tag[] | null;
}
export class GameViewModel extends ViewModel<$models.Game, $apiClients.GameApiClient, number> implements $models.Game  {
  
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


export interface GenreViewModel extends $models.Genre {
  genreId: number | null;
  name: string | null;
  description: string | null;
  games: GameViewModel[] | null;
}
export class GenreViewModel extends ViewModel<$models.Genre, $apiClients.GenreApiClient, number> implements $models.Genre  {
  
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


const viewModelTypeLookup = ViewModel.typeLookup = {
  ApplicationUser: ApplicationUserViewModel,
  Game: GameViewModel,
  Genre: GenreViewModel,
}
const listViewModelTypeLookup = ListViewModel.typeLookup = {
  ApplicationUser: ApplicationUserListViewModel,
  Game: GameListViewModel,
  Genre: GenreListViewModel,
}
const serviceViewModelTypeLookup = ServiceViewModel.typeLookup = {
}

