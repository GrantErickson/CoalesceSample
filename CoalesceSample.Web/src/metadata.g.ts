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
      type: "string",
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
    totalRating: {
      name: "totalRating",
      displayName: "Total Rating",
      type: "number",
      role: "value",
      dontSerialize: true,
    },
    numberOfRatings: {
      name: "numberOfRatings",
      displayName: "Number Of Ratings",
      type: "number",
      role: "value",
      dontSerialize: true,
    },
    averageRating: {
      name: "averageRating",
      displayName: "Average Rating",
      type: "number",
      role: "value",
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
      dontSerialize: true,
    },
    imageId: {
      name: "imageId",
      displayName: "Image Id",
      type: "number",
      role: "foreignKey",
      get principalKey() { return (domain.types.Image as ModelType).props.imageId as PrimaryKeyProperty },
      get principalType() { return (domain.types.Image as ModelType) },
      get navigationProp() { return (domain.types.Game as ModelType).props.image as ModelReferenceNavigationProperty },
      hidden: 3,
      rules: {
        required: val => val != null || "Image is required.",
      }
    },
    image: {
      name: "image",
      displayName: "Image",
      type: "model",
      get typeDef() { return (domain.types.Image as ModelType) },
      role: "referenceNavigation",
      get foreignKey() { return (domain.types.Game as ModelType).props.imageId as ForeignKeyProperty },
      get principalKey() { return (domain.types.Image as ModelType).props.imageId as PrimaryKeyProperty },
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
    gameDataSource: {
      type: "dataSource",
      name: "GameDataSource",
      displayName: "Game Data Source",
      isDefault: true,
      props: {
        filterTags: {
          name: "filterTags",
          displayName: "Filter Tags",
          type: "string",
          role: "value",
        },
        filterRatingsUpper: {
          name: "filterRatingsUpper",
          displayName: "Filter Ratings Upper",
          type: "number",
          role: "value",
        },
        filterRatingsLower: {
          name: "filterRatingsLower",
          displayName: "Filter Ratings Lower",
          type: "number",
          role: "value",
        },
      },
    },
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
      type: "string",
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
  },
  methods: {
  },
  dataSources: {
  },
}
export const Image = domain.types.Image = {
  name: "Image",
  displayName: "Image",
  get displayProp() { return this.props.imageId }, 
  type: "model",
  controllerRoute: "Image",
  get keyProp() { return this.props.imageId }, 
  behaviorFlags: 7,
  props: {
    imageId: {
      name: "imageId",
      displayName: "Image Id",
      type: "number",
      role: "primaryKey",
      hidden: 3,
    },
    content: {
      name: "content",
      displayName: "Content",
      type: "binary",
      base64: true,
      role: "value",
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
    isDeleted: {
      name: "isDeleted",
      displayName: "Is Deleted",
      type: "boolean",
      role: "value",
    },
    gameId: {
      name: "gameId",
      displayName: "Game Id",
      type: "string",
      role: "value",
    },
    reviewedGame: {
      name: "reviewedGame",
      displayName: "Reviewed Game",
      type: "model",
      get typeDef() { return (domain.types.Game as ModelType) },
      role: "value",
      dontSerialize: true,
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
export const UserDetails = domain.types.UserDetails = {
  name: "UserDetails",
  displayName: "User Details",
  get displayProp() { return this.props.id }, 
  type: "model",
  controllerRoute: "UserDetails",
  get keyProp() { return this.props.id }, 
  behaviorFlags: 7,
  props: {
    id: {
      name: "id",
      displayName: "Id",
      type: "string",
      role: "primaryKey",
      hidden: 3,
    },
  },
  methods: {
  },
  dataSources: {
  },
}
export const UserInfoDto = domain.types.UserInfoDto = {
  name: "UserInfoDto",
  displayName: "User Info Dto",
  get displayProp() { return this.props.name }, 
  type: "object",
  props: {
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
    userRoles: {
      name: "userRoles",
      displayName: "User Roles",
      type: "collection",
      itemType: {
        name: "$collectionItem",
        displayName: "",
        role: "value",
        type: "string",
      },
      role: "value",
    },
  },
}
export const ApplicationUserService = domain.services.ApplicationUserService = {
  name: "ApplicationUserService",
  displayName: "Application User Service",
  type: "service",
  controllerRoute: "ApplicationUserService",
  methods: {
    getRoles: {
      name: "getRoles",
      displayName: "Get Roles",
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
          type: "string",
        },
        role: "value",
      },
    },
    hasRole: {
      name: "hasRole",
      displayName: "Has Role",
      transportType: "item",
      httpMethod: "POST",
      params: {
        role: {
          name: "role",
          displayName: "Role",
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
    getUserReviews: {
      name: "getUserReviews",
      displayName: "Get User Reviews",
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
          type: "string",
        },
        role: "value",
      },
    },
    getAllUsersInfo: {
      name: "getAllUsersInfo",
      displayName: "Get All Users Info",
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
          type: "object",
          get typeDef() { return (domain.types.UserInfoDto as ObjectType) },
        },
        role: "value",
      },
    },
    getRoleList: {
      name: "getRoleList",
      displayName: "Get Role List",
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
          type: "string",
        },
        role: "value",
      },
    },
    toggleUserRole: {
      name: "toggleUserRole",
      displayName: "Toggle User Role",
      transportType: "item",
      httpMethod: "POST",
      params: {
        userEmail: {
          name: "userEmail",
          displayName: "User Email",
          type: "string",
          role: "value",
        },
        role: {
          name: "role",
          displayName: "Role",
          type: "string",
          role: "value",
        },
        currentState: {
          name: "currentState",
          displayName: "Current State",
          type: "boolean",
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
export const GameService = domain.services.GameService = {
  name: "GameService",
  displayName: "Game Service",
  type: "service",
  controllerRoute: "GameService",
  methods: {
    getGameDetails: {
      name: "getGameDetails",
      displayName: "Get Game Details",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "string",
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
    getGameImage: {
      name: "getGameImage",
      displayName: "Get Game Image",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "string",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "model",
        get typeDef() { return (domain.types.Image as ModelType) },
        role: "value",
      },
    },
    uploadGameImage: {
      name: "uploadGameImage",
      displayName: "Upload Game Image",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "string",
          role: "value",
        },
        image: {
          name: "image",
          displayName: "Image",
          type: "file",
          role: "value",
        },
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "model",
        get typeDef() { return (domain.types.Image as ModelType) },
        role: "value",
      },
    },
    getGameTags: {
      name: "getGameTags",
      displayName: "Get Game Tags",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "string",
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
          get typeDef() { return (domain.types.GameTag as ModelType) },
        },
        role: "value",
      },
    },
    setGameTags: {
      name: "setGameTags",
      displayName: "Set Game Tags",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
          type: "string",
          role: "value",
        },
        tagIds: {
          name: "tagIds",
          displayName: "Tag Ids",
          type: "collection",
          itemType: {
            name: "$collectionItem",
            displayName: "",
            role: "value",
            type: "number",
          },
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
          get typeDef() { return (domain.types.GameTag as ModelType) },
        },
        role: "value",
      },
    },
    addLike: {
      name: "addLike",
      displayName: "Add Like",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
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
    removeLike: {
      name: "removeLike",
      displayName: "Remove Like",
      transportType: "item",
      httpMethod: "POST",
      params: {
        gameId: {
          name: "gameId",
          displayName: "Game Id",
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
    getUserInfo: {
      name: "getUserInfo",
      displayName: "Get User Info",
      transportType: "item",
      httpMethod: "POST",
      params: {
      },
      return: {
        name: "$return",
        displayName: "Result",
        type: "object",
        get typeDef() { return (domain.types.UserInfoDto as ObjectType) },
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
          type: "string",
          role: "value",
        },
        firstDate: {
          name: "firstDate",
          displayName: "First Date",
          type: "date",
          dateKind: "datetime",
          noOffset: true,
          role: "value",
        },
        secondDate: {
          name: "secondDate",
          displayName: "Second Date",
          type: "date",
          dateKind: "datetime",
          noOffset: true,
          role: "value",
        },
        page: {
          name: "page",
          displayName: "Page",
          type: "number",
          role: "value",
        },
        reviewsPerPage: {
          name: "reviewsPerPage",
          displayName: "Reviews Per Page",
          type: "number",
          role: "value",
        },
        minRating: {
          name: "minRating",
          displayName: "Min Rating",
          type: "number",
          role: "value",
        },
        maxRating: {
          name: "maxRating",
          displayName: "Max Rating",
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
        type: "model",
        get typeDef() { return (domain.types.Review as ModelType) },
        role: "value",
      },
    },
    deleteReview: {
      name: "deleteReview",
      displayName: "Delete Review",
      transportType: "item",
      httpMethod: "POST",
      params: {
        reviewId: {
          name: "reviewId",
          displayName: "Review Id",
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
  },
}

interface AppDomain extends Domain {
  enums: {
  }
  types: {
    Game: typeof Game
    GameTag: typeof GameTag
    Genre: typeof Genre
    Image: typeof Image
    Review: typeof Review
    Tag: typeof Tag
    UserDetails: typeof UserDetails
    UserInfoDto: typeof UserInfoDto
  }
  services: {
    ApplicationUserService: typeof ApplicationUserService
    GameService: typeof GameService
    LoginService: typeof LoginService
    ReviewService: typeof ReviewService
  }
}

solidify(domain)

export default domain as unknown as AppDomain
