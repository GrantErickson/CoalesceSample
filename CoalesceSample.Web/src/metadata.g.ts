import {
  Domain, getEnumMeta, solidify, ModelType, ObjectType,
  PrimitiveProperty, ForeignKeyProperty, PrimaryKeyProperty,
  ModelCollectionNavigationProperty, ModelReferenceNavigationProperty
} from 'coalesce-vue/lib/metadata'


const domain: Domain = { enums: {}, types: {}, services: {} }
export const Game = domain.types.Game = {
  name: "Game",
  displayName: "Game",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Game",
  get keyProp() { return this.props.gameId }, 
  behaviorFlags: 7,
  props: {
    gameId: {
      name: "gameId",
      displayName: "Game Id",
      type: "number",
      role: "primaryKey",
      hidden: 3,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    releaseDate: {
      name: "releaseDate",
      displayName: "Release Date",
      type: "date",
      dateKind: "datetime",
      noOffset: true,
      role: "value",
    },
    likes: {
      name: "likes",
      displayName: "Likes",
      type: "number",
      role: "value",
    },
    numberOfRatings: {
      name: "numberOfRatings",
      displayName: "Number Of Ratings",
      type: "number",
      role: "value",
    },
    averageRating: {
      name: "averageRating",
      displayName: "Average Rating",
      type: "number",
      role: "value",
      dontSerialize: true,
    },
    averageDurationInHours: {
      name: "averageDurationInHours",
      displayName: "Average Duration In Hours",
      type: "number",
      role: "value",
    },
    maxPlayers: {
      name: "maxPlayers",
      displayName: "Max Players",
      type: "number",
      role: "value",
    },
    minPlayers: {
      name: "minPlayers",
      displayName: "Min Players",
      type: "number",
      role: "value",
    },
    genreId: {
      name: "genreId",
      displayName: "Genre Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.Genre as ModelType).props.genreId as PrimaryKeyProperty },
      get principalType() { return (domain.types.Genre as ModelType) },
      get navigationProp() { return (domain.types.Game as ModelType).props.genre as ModelReferenceNavigationProperty },
      hidden: 3,
      rules: {
        required: val => val != null || "Genre is required.",
      }
    },
    genre: {
      name: "genre",
      displayName: "Genre",
      type: "model",
      get typeDef() { return (domain.types.Genre as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.Game as ModelType).props.genreId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Genre as ModelType).props.genreId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.Genre as ModelType).props.games as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
    gameTags: {
      name: "gameTags",
      displayName: "Game Tags",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.GameTag as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.GameTag as ModelType).props.gameId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.GameTag as ModelType).props.game as ModelReferenceNavigationProperty },
      manyToMany: {
        name: "tag",
        displayName: "Tag",
        get typeDef() { return (domain.types.Tag as ModelType) },
        get farForeignKey() { return (domain.types.GameTag as ModelType).props.tagId as ForeignKeyProperty },
        get farNavigationProp() { return (domain.types.GameTag as ModelType).props.tag as ModelReferenceNavigationProperty },
        get nearForeignKey() { return (domain.types.GameTag as ModelType).props.gameId as ForeignKeyProperty },
        get nearNavigationProp() { return (domain.types.GameTag as ModelType).props.game as ModelReferenceNavigationProperty },
      },
      dontSerialize: true,
    },
    reviews: {
      name: "reviews",
      displayName: "Reviews",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.Review as ModelType) },
      },
      role: "value",
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const GameTag = domain.types.GameTag = {
  name: "GameTag",
  displayName: "Game Tag",
  get displayProp() { return this.props.gameTagId }, 
  type: "model",
  controllerRoute: "GameTag",
  get keyProp() { return this.props.gameTagId }, 
  behaviorFlags: 7,
  props: {
    gameTagId: {
      name: "gameTagId",
      displayName: "Game Tag Id",
      type: "number",
      role: "primaryKey",
      hidden: 3,
    },
    tagId: {
      name: "tagId",
      displayName: "Tag Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.Tag as ModelType).props.tagId as PrimaryKeyProperty },
      get principalType() { return (domain.types.Tag as ModelType) },
      get navigationProp() { return (domain.types.GameTag as ModelType).props.tag as ModelReferenceNavigationProperty },
      hidden: 3,
      rules: {
        required: val => val != null || "Tag is required.",
      }
    },
    tag: {
      name: "tag",
      displayName: "Tag",
      type: "model",
      get typeDef() { return (domain.types.Tag as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.GameTag as ModelType).props.tagId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Tag as ModelType).props.tagId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.Tag as ModelType).props.gameTags as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
    gameId: {
      name: "gameId",
      displayName: "Game Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.Game as ModelType).props.gameId as PrimaryKeyProperty },
      get principalType() { return (domain.types.Game as ModelType) },
      get navigationProp() { return (domain.types.GameTag as ModelType).props.game as ModelReferenceNavigationProperty },
      hidden: 3,
      rules: {
        required: val => val != null || "Game is required.",
      }
    },
    game: {
      name: "game",
      displayName: "Game",
      type: "model",
      get typeDef() { return (domain.types.Game as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.GameTag as ModelType).props.gameId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Game as ModelType).props.gameId as PrimaryKeyProperty },
      get inverseNavigation() { return (domain.types.Game as ModelType).props.gameTags as ModelCollectionNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Genre = domain.types.Genre = {
  name: "Genre",
  displayName: "Genre",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Genre",
  get keyProp() { return this.props.genreId }, 
  behaviorFlags: 7,
  props: {
    genreId: {
      name: "genreId",
      displayName: "Genre Id",
      type: "number",
      role: "primaryKey",
      hidden: 3,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    games: {
      name: "games",
      displayName: "Games",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.Game as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.Game as ModelType).props.genreId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.Game as ModelType).props.genre as ModelReferenceNavigationProperty },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Review = domain.types.Review = {
  name: "Review",
  displayName: "Review",
  get displayProp() { return this.props.reviewId }, 
  type: "model",
  controllerRoute: "Review",
  get keyProp() { return this.props.reviewId }, 
  behaviorFlags: 7,
  props: {
    reviewId: {
      name: "reviewId",
      displayName: "Review Id",
      type: "string",
      role: "primaryKey",
      hidden: 3,
    },
    rating: {
      name: "rating",
      displayName: "Rating",
      type: "number",
      role: "value",
    },
    reviewDate: {
      name: "reviewDate",
      displayName: "Review Date",
      type: "date",
      dateKind: "datetime",
      noOffset: true,
      role: "value",
    },
    reviewerName: {
      name: "reviewerName",
      displayName: "Reviewer Name",
      type: "string",
      role: "value",
    },
    reviewTitle: {
      name: "reviewTitle",
      displayName: "Review Title",
      type: "string",
      role: "value",
    },
    reviewBody: {
      name: "reviewBody",
      displayName: "Review Body",
      type: "string",
      role: "value",
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const Tag = domain.types.Tag = {
  name: "Tag",
  displayName: "Tag",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "Tag",
  get keyProp() { return this.props.tagId }, 
  behaviorFlags: 7,
  props: {
    tagId: {
      name: "tagId",
      displayName: "Tag Id",
      type: "number",
      role: "primaryKey",
      hidden: 3,
    },
    name: {
      name: "name",
      displayName: "Name",
      type: "string",
      role: "value",
    },
    description: {
      name: "description",
      displayName: "Description",
      type: "string",
      role: "value",
    },
    gameTags: {
      name: "gameTags",
      displayName: "Game Tags",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "model",
        get typeDef() { return (domain.types.GameTag as ModelType) },
      },
      role: "collectionNavigation",
      get foreignKey() { return (domain.types.GameTag as ModelType).props.tagId as ForeignKeyProperty },
      get inverseNavigation() { return (domain.types.GameTag as ModelType).props.tag as ModelReferenceNavigationProperty },
      manyToMany: {
        name: "game",
        displayName: "Game",
        get typeDef() { return (domain.types.Game as ModelType) },
        get farForeignKey() { return (domain.types.GameTag as ModelType).props.gameId as ForeignKeyProperty },
        get farNavigationProp() { return (domain.types.GameTag as ModelType).props.game as ModelReferenceNavigationProperty },
        get nearForeignKey() { return (domain.types.GameTag as ModelType).props.tagId as ForeignKeyProperty },
        get nearNavigationProp() { return (domain.types.GameTag as ModelType).props.tag as ModelReferenceNavigationProperty },
      },
      dontSerialize: true,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const GameService = domain.services.GameService = {
  name: "GameService",
  displayName: "Game Service",
  type: "service",
  controllerRoute: "GameService",
  methods: {
    getGames: {
      name: "getGames",
      displayName: "Get Games",
      transportType: "item",
      httpMethod: "POST",
      params: {
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "collection",
        itemType: {
          name: "$collectionItem",
          displayName: "",
          role: "value",
          type: "model",
          get typeDef() { return (domain.types.Game as ModelType) },
        },
        role: "value",
      },
    },
    getGameDetails: {
      name: "getGameDetails",
      displayName: "Get Game Details",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "number",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "model",
        get typeDef() { return (domain.types.Game as ModelType) },
        role: "value",
      },
    },
    likeGame: {
      name: "likeGame",
      displayName: "Like Game",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "number",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    getGameImage: {
      name: "getGameImage",
      displayName: "Get Game Image",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "number",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        // Type not supported natively by Coalesce - falling back to unknown.
        type: "unknown",
        role: "value",
      },
    },
  },
}
export const LoginService = domain.services.LoginService = {
  name: "LoginService",
  displayName: "Login Service",
  type: "service",
  controllerRoute: "LoginService",
  methods: {
    login: {
      name: "login",
      displayName: "Login",
      transportType: "item",
      httpMethod: "POST",
      params: {
        email: {
          name: "email",
          displayName: "Email",
          type: "string",
          role: "value",
        },
        password: {
          name: "password",
          displayName: "Password",
          type: "string",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    getToken: {
      name: "getToken",
      displayName: "Get Token",
      transportType: "item",
      httpMethod: "POST",
      params: {
        email: {
          name: "email",
          displayName: "Email",
          type: "string",
          role: "value",
        },
        password: {
          name: "password",
          displayName: "Password",
          type: "string",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        // Type not supported natively by Coalesce - falling back to unknown.
        type: "unknown",
        role: "value",
      },
    },
    logout: {
      name: "logout",
      displayName: "Logout",
      transportType: "item",
      httpMethod: "POST",
      params: {
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    createAccount: {
      name: "createAccount",
      displayName: "Create Account",
      transportType: "item",
      httpMethod: "POST",
      params: {
        name: {
          name: "name",
          displayName: "Name",
          type: "string",
          role: "value",
        },
        email: {
          name: "email",
          displayName: "Email",
          type: "string",
          role: "value",
        },
        password: {
          name: "password",
          displayName: "Password",
          type: "string",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    changePassword: {
      name: "changePassword",
      displayName: "Change Password",
      transportType: "item",
      httpMethod: "POST",
      params: {
        currentPassword: {
          name: "currentPassword",
          displayName: "Current Password",
          type: "string",
          role: "value",
        },
        newPassword: {
          name: "newPassword",
          displayName: "New Password",
          type: "string",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
    isLoggedIn: {
      name: "isLoggedIn",
      displayName: "Is Logged In",
      transportType: "item",
      httpMethod: "POST",
      params: {
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
  },
}
export const ReviewService = domain.services.ReviewService = {
  name: "ReviewService",
  displayName: "Review Service",
  type: "service",
  controllerRoute: "ReviewService",
  methods: {
    getReviews: {
      name: "getReviews",
      displayName: "Get Reviews",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "number",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "collection",
        itemType: {
          name: "$collectionItem",
          displayName: "",
          role: "value",
          type: "model",
          get typeDef() { return (domain.types.Review as ModelType) },
        },
        role: "value",
      },
    },
    addReview: {
      name: "addReview",
      displayName: "Add Review",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "number",
          role: "value",
        },
        reviewTitle: {
          name: "reviewTitle",
          displayName: "Review Title",
          type: "string",
          role: "value",
        },
        reviewBody: {
          name: "reviewBody",
          displayName: "Review Body",
          type: "string",
          role: "value",
        },
        rating: {
          name: "rating",
          displayName: "Rating",
          type: "number",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "void",
        role: "value",
      },
    },
  },
}

interface AppDomain extends Domain {
  enums: {
  }
  types: {
    Game: typeof Game
    GameTag: typeof GameTag
    Genre: typeof Genre
    Review: typeof Review
    Tag: typeof Tag
  }
  services: {
    GameService: typeof GameService
    LoginService: typeof LoginService
    ReviewService: typeof ReviewService
  }
}

solidify(domain)

export default domain as unknown as AppDomain
