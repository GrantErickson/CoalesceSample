import * as $metadata from './metadata.g'
import * as $models from './models.g'
import { AxiosClient, ModelApiClient, ServiceApiClient, ItemResult, ListResult } from 'coalesce-vue/lib/api-client'
import { AxiosPromise, AxiosResponse, AxiosRequestConfig } from 'axios'

export class GameApiClient extends ModelApiClient<$models.Game> {
  constructor() { super($metadata.Game) }
}


export class GameTagApiClient extends ModelApiClient<$models.GameTag> {
  constructor() { super($metadata.GameTag) }
}


export class GenreApiClient extends ModelApiClient<$models.Genre> {
  constructor() { super($metadata.Genre) }
}


export class TagApiClient extends ModelApiClient<$models.Tag> {
  constructor() { super($metadata.Tag) }
}


export class GameServiceApiClient extends ServiceApiClient<typeof $metadata.GameService> {
  constructor() { super($metadata.GameService) }
  public getGames($config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Game[]>> {
    const $method = this.$metadata.methods.getGames
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
}


export class LoginServiceApiClient extends ServiceApiClient<typeof $metadata.LoginService> {
  constructor() { super($metadata.LoginService) }
  public login(email: string | null, password: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.login
    const $params =  {
      email,
      password,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getToken(email: string | null, password: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<unknown>> {
    const $method = this.$metadata.methods.getToken
    const $params =  {
      email,
      password,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public logout($config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.logout
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
  public createAccount(name: string | null, email: string | null, password: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.createAccount
    const $params =  {
      name,
      email,
      password,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public changePassword(currentPassword: string | null, newPassword: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.changePassword
    const $params =  {
      currentPassword,
      newPassword,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public isLoggedIn($config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.isLoggedIn
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
}


