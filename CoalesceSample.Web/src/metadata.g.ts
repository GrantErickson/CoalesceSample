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
  },
  methods: {
  },
  dataSources: {
  },
}

interface AppDomain extends Domain {
  enums: {
  }
  types: {
    ApplicationUser: typeof ApplicationUser
    Game: typeof Game
  }
  services: {
  }
}

solidify(domain)

export default domain as unknown as AppDomain
