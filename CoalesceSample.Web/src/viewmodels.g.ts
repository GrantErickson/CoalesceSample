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


const viewModelTypeLookup = ViewModel.typeLookup = {
  ApplicationUser: ApplicationUserViewModel,
  Game: GameViewModel,
}
const listViewModelTypeLookup = ListViewModel.typeLookup = {
  ApplicationUser: ApplicationUserListViewModel,
  Game: GameListViewModel,
}
const serviceViewModelTypeLookup = ServiceViewModel.typeLookup = {
}

