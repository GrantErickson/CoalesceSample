import {
  Domain, getEnumMeta, solidify, ModelType, ObjectType,
  PrimitiveProperty, ForeignKeyProperty, PrimaryKeyProperty,
  ModelCollectionNavigationProperty, ModelReferenceNavigationProperty
} from 'coalesce-vue/lib/metadata'


const domain: Domain = { enums: {}, types: {}, services: {} }
export const ApplicationUser = domain.types.ApplicationUser = {
  name: "ApplicationUser",
  displayName: "Application User",
  get displayProp() { return this.props.name }, 
  type: "model",
  controllerRoute: "ApplicationUser",
  get keyProp() { return this.props.applicationUserId }, 
  behaviorFlags: 7,
  props: {
    applicationUserId: {
      name: "applicationUserId",
      displayName: "Application User Id",
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
  },
  methods: {
  },
  dataSources: {
  },
}
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
        type: "object",
        get typeDef() { return (domain.types.Tag as ObjectType) },
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
  type: "object",
  props: {
    tagId: {
      name: "tagId",
      displayName: "Tag Id",
      type: "number",
      role: "value",
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
      role: "value",
      dontSerialize: true,
    },
  },
}

interface AppDomain extends Domain {
  enums: {
  }
  types: {
    ApplicationUser: typeof ApplicationUser
    Game: typeof Game
    Genre: typeof Genre
    Tag: typeof Tag
  }
  services: {
  }
}

solidify(domain)

export default domain as unknown as AppDomain
