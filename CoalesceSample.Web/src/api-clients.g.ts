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


export class ImageApiClient extends ModelApiClient<$models.Image> {
  constructor() { super($metadata.Image) }
}


export class ReviewApiClient extends ModelApiClient<$models.Review> {
  constructor() { super($metadata.Review) }
}


export class TagApiClient extends ModelApiClient<$models.Tag> {
  constructor() { super($metadata.Tag) }
}


export class ApplicationUserServiceApiClient extends ServiceApiClient<typeof $metadata.ApplicationUserService> {
  constructor() { super($metadata.ApplicationUserService) }
  public getRoles($config?: AxiosRequestConfig): AxiosPromise<ItemResult<string[]>> {
    const $method = this.$metadata.methods.getRoles
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
  public hasRole(role: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.hasRole
    const $params =  {
      role,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getUserReviews($config?: AxiosRequestConfig): AxiosPromise<ItemResult<string[]>> {
    const $method = this.$metadata.methods.getUserReviews
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
  public isLoggedIn($config?: AxiosRequestConfig): AxiosPromise<ItemResult<unknown>> {
    const $method = this.$metadata.methods.isLoggedIn
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
}


export class GameServiceApiClient extends ServiceApiClient<typeof $metadata.GameService> {
  constructor() { super($metadata.GameService) }
  public getGames($config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Game[]>> {
    const $method = this.$metadata.methods.getGames
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getGamesFromIds(gameIds: string[] | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Game[]>> {
    const $method = this.$metadata.methods.getGamesFromIds
    const $params =  {
      gameIds,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getGameDetails(gameId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Game>> {
    const $method = this.$metadata.methods.getGameDetails
    const $params =  {
      gameId,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getGameImage(gameId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<string>> {
    const $method = this.$metadata.methods.getGameImage
    const $params =  {
      gameId,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public uploadGameImage(gameId: string | null, image: File | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<File>> {
    const $method = this.$metadata.methods.uploadGameImage
    const $params =  {
      gameId,
      image,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getAllTags($config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Tag[]>> {
    const $method = this.$metadata.methods.getAllTags
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
  public getGameTags(gameId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.GameTag[]>> {
    const $method = this.$metadata.methods.getGameTags
    const $params =  {
      gameId,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public setGameTags(gameId: string | null, tagIds: number[] | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.GameTag[]>> {
    const $method = this.$metadata.methods.setGameTags
    const $params =  {
      gameId,
      tagIds,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public addLike(gameId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.addLike
    const $params =  {
      gameId,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public removeLike(gameId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.removeLike
    const $params =  {
      gameId,
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
  
  public getUserInfo($config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.UserInfoDto>> {
    const $method = this.$metadata.methods.getUserInfo
    const $params =  {
    }
    return this.$invoke($method, $params, $config)
  }
  
}


export class ReviewServiceApiClient extends ServiceApiClient<typeof $metadata.ReviewService> {
  constructor() { super($metadata.ReviewService) }
  public getReviews(gameId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Review[]>> {
    const $method = this.$metadata.methods.getReviews
    const $params =  {
      gameId,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public addReview(gameId: string | null, reviewTitle: string | null, reviewBody: string | null, rating: number | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<$models.Review>> {
    const $method = this.$metadata.methods.addReview
    const $params =  {
      gameId,
      reviewTitle,
      reviewBody,
      rating,
    }
    return this.$invoke($method, $params, $config)
  }
  
  public deleteReview(reviewId: string | null, $config?: AxiosRequestConfig): AxiosPromise<ItemResult<void>> {
    const $method = this.$metadata.methods.deleteReview
    const $params =  {
      reviewId,
    }
    return this.$invoke($method, $params, $config)
  }
  
}


