using MiNET.Net;

namespace Alex.Services
{
    public class NullMcpeClientMessageHandler : IMcpeClientMessageHandler
    {
        public virtual void HandleMcpePlayStatus(McpePlayStatus message) { }

        public virtual void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message) { }

        public virtual void HandleMcpeDisconnect(McpeDisconnect message) { }

        public virtual void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message) { }

        public virtual void HandleMcpeResourcePackStack(McpeResourcePackStack message) { }

        public virtual void HandleMcpeText(McpeText message) { }

        public virtual void HandleMcpeSetTime(McpeSetTime message) { }

        public virtual void HandleMcpeStartGame(McpeStartGame message) { }

        public virtual void HandleMcpeAddPlayer(McpeAddPlayer message) { }

        public virtual void HandleMcpeAddEntity(McpeAddEntity message) { }

        public virtual void HandleMcpeRemoveEntity(McpeRemoveEntity message) { }

        public virtual void HandleMcpeAddItemEntity(McpeAddItemEntity message) { }

        public virtual void HandleMcpeTakeItemEntity(McpeTakeItemEntity message) { }

        public virtual void HandleMcpeMoveEntity(McpeMoveEntity message) { }

        public virtual void HandleMcpeMovePlayer(McpeMovePlayer message) { }

        public virtual void HandleMcpeRiderJump(McpeRiderJump message) { }

        public virtual void HandleMcpeUpdateBlock(McpeUpdateBlock message) { }

        public virtual void HandleMcpeAddPainting(McpeAddPainting message) { }

        public virtual void HandleMcpeTickSync(McpeTickSync message) { }

        public virtual void HandleMcpeLevelSoundEventOld(McpeLevelSoundEventOld message) { }

        public virtual void HandleMcpeLevelEvent(McpeLevelEvent message) { }

        public virtual void HandleMcpeBlockEvent(McpeBlockEvent message) { }

        public virtual void HandleMcpeEntityEvent(McpeEntityEvent message) { }

        public virtual void HandleMcpeMobEffect(McpeMobEffect message) { }

        public virtual void HandleMcpeUpdateAttributes(McpeUpdateAttributes message) { }

        public virtual void HandleMcpeInventoryTransaction(McpeInventoryTransaction message) { }

        public virtual void HandleMcpeMobEquipment(McpeMobEquipment message) { }

        public virtual void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message) { }

        public virtual void HandleMcpeInteract(McpeInteract message) { }

        public virtual void HandleMcpeHurtArmor(McpeHurtArmor message) { }

        public virtual void HandleMcpeSetEntityData(McpeSetEntityData message) { }

        public virtual void HandleMcpeSetEntityMotion(McpeSetEntityMotion message) { }

        public virtual void HandleMcpeSetEntityLink(McpeSetEntityLink message) { }

        public virtual void HandleMcpeSetHealth(McpeSetHealth message) { }

        public virtual void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message) { }

        public virtual void HandleMcpeAnimate(McpeAnimate message) { }

        public virtual void HandleMcpeRespawn(McpeRespawn message) { }

        public virtual void HandleMcpeContainerOpen(McpeContainerOpen message) { }

        public virtual void HandleMcpeContainerClose(McpeContainerClose message) { }

        public virtual void HandleMcpePlayerHotbar(McpePlayerHotbar message) { }

        public virtual void HandleMcpeInventoryContent(McpeInventoryContent message) { }

        public virtual void HandleMcpeInventorySlot(McpeInventorySlot message) { }

        public virtual void HandleMcpeContainerSetData(McpeContainerSetData message) { }

        public virtual void HandleMcpeCraftingData(McpeCraftingData message) { }

        public virtual void HandleMcpeCraftingEvent(McpeCraftingEvent message) { }

        public virtual void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message) { }

        public virtual void HandleMcpeAdventureSettings(McpeAdventureSettings message) { }

        public virtual void HandleMcpeBlockEntityData(McpeBlockEntityData message) { }

        public virtual void HandleMcpeLevelChunk(McpeLevelChunk message) { }

        public virtual void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message) { }

        public virtual void HandleMcpeSetDifficulty(McpeSetDifficulty message) { }

        public virtual void HandleMcpeChangeDimension(McpeChangeDimension message) { }

        public virtual void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message) { }

        public virtual void HandleMcpePlayerList(McpePlayerList message) { }

        public virtual void HandleMcpeSimpleEvent(McpeSimpleEvent message) { }

        public virtual void HandleMcpeTelemetryEvent(McpeTelemetryEvent message) { }

        public virtual void HandleMcpeSpawnExperienceOrb(McpeSpawnExperienceOrb message) { }

        public virtual void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message) { }

        public virtual void HandleMcpeMapInfoRequest(McpeMapInfoRequest message) { }

        public virtual void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message) { }

        public virtual void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message) { }

        public virtual void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message) { }

        public virtual void HandleMcpeGameRulesChanged(McpeGameRulesChanged message) { }

        public virtual void HandleMcpeCamera(McpeCamera message) { }

        public virtual void HandleMcpeBossEvent(McpeBossEvent message) { }

        public virtual void HandleMcpeShowCredits(McpeShowCredits message) { }

        public virtual void HandleMcpeAvailableCommands(McpeAvailableCommands message) { }

        public virtual void HandleMcpeCommandOutput(McpeCommandOutput message) { }

        public virtual void HandleMcpeUpdateTrade(McpeUpdateTrade message) { }

        public virtual void HandleMcpeUpdateEquipment(McpeUpdateEquipment message) { }

        public virtual void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message) { }

        public virtual void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message) { }

        public virtual void HandleMcpeTransfer(McpeTransfer message) { }

        public virtual void HandleMcpePlaySound(McpePlaySound message) { }

        public virtual void HandleMcpeStopSound(McpeStopSound message) { }

        public virtual void HandleMcpeSetTitle(McpeSetTitle message) { }

        public virtual void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message) { }

        public virtual void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message) { }

        public virtual void HandleMcpeShowStoreOffer(McpeShowStoreOffer message) { }

        public virtual void HandleMcpePlayerSkin(McpePlayerSkin message) { }

        public virtual void HandleMcpeSubClientLogin(McpeSubClientLogin message) { }

        public virtual void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message) { }

        public virtual void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message) { }

        public virtual void HandleMcpeBookEdit(McpeBookEdit message) { }

        public virtual void HandleMcpeNpcRequest(McpeNpcRequest message) { }

        public virtual void HandleMcpeModalFormRequest(McpeModalFormRequest message) { }

        public virtual void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message) { }

        public virtual void HandleMcpeShowProfile(McpeShowProfile message) { }

        public virtual void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message) { }

        public virtual void HandleMcpeRemoveObjective(McpeRemoveObjective message) { }

        public virtual void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message) { }

        public virtual void HandleMcpeSetScore(McpeSetScore message) { }

        public virtual void HandleMcpeLabTable(McpeLabTable message) { }

        public virtual void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message) { }

        public virtual void HandleMcpeMoveEntityDelta(McpeMoveEntityDelta message) { }

        public virtual void HandleMcpeSetScoreboardIdentityPacket(McpeSetScoreboardIdentityPacket message) { }

        public virtual void HandleMcpeUpdateSoftEnumPacket(McpeUpdateSoftEnumPacket message) { }

        public virtual void HandleMcpeNetworkStackLatencyPacket(McpeNetworkStackLatencyPacket message) { }

        public virtual void HandleMcpeScriptCustomEventPacket(McpeScriptCustomEventPacket message) { }

        public virtual void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message) { }

        public virtual void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message) { }

        public virtual void HandleMcpeLevelSoundEventV2(McpeLevelSoundEventV2 message) { }

        public virtual void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message) { }

        public virtual void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message) { }

        public virtual void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message) { }

        public virtual void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message) { }

        public virtual void HandleMcpeLecternUpdate(McpeLecternUpdate message) { }

        public virtual void HandleMcpeVideoStreamConnect(McpeVideoStreamConnect message) { }

        public virtual void HandleMcpeClientCacheStatus(McpeClientCacheStatus message) { }

        public virtual void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message) { }

        public virtual void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message) { }

        public virtual void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataExportRequest message) { }

        public virtual void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataExportResponse message) { }

        public virtual void HandleMcpeUpdateBlockProperties(McpeUpdateBlockProperties message) { }

        public virtual void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message) { }

        public virtual void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message) { }

        public virtual void HandleMcpeNetworkSettingsPacket(McpeNetworkSettingsPacket message) { }

        public virtual void HandleFtlCreatePlayer(FtlCreatePlayer message) { }
    }
}